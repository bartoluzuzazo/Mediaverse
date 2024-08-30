import { createFileRoute } from '@tanstack/react-router'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntrySectionHeader from '../../../common/components/entries/entrySectionHeader.tsx'
import { useQuery } from '@tanstack/react-query'
import { bookService } from '../../../services/bookService.ts'
import { Book } from '../../../models/entry/book/Book.ts'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'

export const Route = createFileRoute('/entries/books/$id')({
  component: () => {
    // const queryClient = useQueryClient()
    const { id } = Route.useParams()

    const {
      data: book,
      isLoading,
      refetch,
    } = useQuery({
      queryFn: async (): Promise<Book> => {
        const res = await bookService.getBook(id)
        return res.data
      },
      queryKey: ['GET_BOOK'],
    })
    if (isLoading) {
      return <div>Loading...</div>
    }

    if (book == undefined) {
      return <div>An error occured</div>
    }
    console.log(book.entry.usersRating)

    const info = [book.entry.release.toString(), ...book.bookGenres]
    return (
      <>
        <EntryBanner entry={book.entry} info={info} type={'Book'} />
        <EntryRatingPicker<Book> entryId={book.entry.id} refetch={refetch} />
        <EntrySectionHeader title={'Description'} />
        <div className="p-4">{book.entry.description}</div>
        <EntrySectionHeader title={'Authors'} />
      </>
    )
  },
})
