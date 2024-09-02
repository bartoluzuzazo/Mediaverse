import { createFileRoute } from '@tanstack/react-router'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntrySectionHeader from '../../../common/components/entries/entrySectionHeader.tsx'
import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { bookService } from '../../../services/bookService.ts'
import { Book } from '../../../models/entry/book/Book.ts'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'
import { useLocalStorage } from 'usehooks-ts'

const bookQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_BOOK', id],
    queryFn: async (): Promise<Book> => {
      const res = await bookService.getBook(id)
      return res.data
    },
  })
}

export const Route = createFileRoute('/entries/books/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(bookQueryOptions(id))
  },

  component: () => {
    // TODO: replace with something that is not bad
    const [token] = useLocalStorage<string | undefined>('token', undefined)
    const isAuthorized = !!token

    const id = Route.useParams().id
    const bookQuery = useSuspenseQuery(bookQueryOptions(id))
    const book = bookQuery.data
    const info = [book.entry.release.toString(), ...book.bookGenres]
    return (
      <>
        <EntryBanner entry={book.entry} info={info} type={'Book'} />
        {isAuthorized ? <EntryRatingPicker entryId={book.entry.id} /> : null}
        <EntrySectionHeader title={'Description'} />
        <div className="p-4">{book.entry.description}</div>
        <EntrySectionHeader title={'Authors'} />
      </>
    )
  },
})
