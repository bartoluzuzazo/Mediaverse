import { createFileRoute } from '@tanstack/react-router'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntrySectionHeader from '../../../common/components/entries/entrySectionHeader.tsx'
import { useQuery } from '@tanstack/react-query'
import { bookService } from '../../../services/bookService.ts'
import { Book } from '../../../models/entry/book/Book.ts'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'
import { useLocalStorage } from 'usehooks-ts'

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

    // TODO: replace with something that is not bad
    const [token, _] = useLocalStorage<string | undefined>('token', undefined)
    const isAuthorized = !!token

    if (isLoading) {
      return <div>Loading...</div>
    }

    if (book == undefined) {
      return <div>An error occured</div>
    }

    const info = [book.entry.release.toString(), ...book.bookGenres]
    return (
      <>
        <EntryBanner entry={book.entry} info={info} type={'Book'} />
        {isAuthorized ? (
          <EntryRatingPicker<Book> entryId={book.entry.id} refetch={refetch} />
        ) : null}
        <EntrySectionHeader title={'Description'} />
        <div className="p-4">{book.entry.description}</div>
        <EntrySectionHeader title={'Authors'} />
      </>
    )
  },
})
