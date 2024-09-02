import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntrySectionHeader from '../../../common/components/entries/entrySectionHeader.tsx'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { Book } from '../../../models/entry/book/index.ts'
import { BookService } from '../../../services/bookService.ts'

interface BookEntryComponentProps {}

const bookQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_BOOK', id],
    queryFn: async (): Promise<Book> => {
      const res = await BookService.getBook(id)
      return res.data
    },
  })
}

const BookEntryComponent: FunctionComponent<BookEntryComponentProps> = () => {
  const authContext = useAuthContext()

  const id = Route.useParams().id
  const bookQuery = useSuspenseQuery(bookQueryOptions(id))
  const book = bookQuery.data
  const info = [book.entry.release.toString(), ...book.bookGenres]

  return (
    <>
      <EntryBanner entry={book.entry} info={info} type={'Book'} />
      {authContext?.isAuthenticated ? (
        <EntryRatingPicker entryId={book.entry.id} />
      ) : null}
      <EntrySectionHeader title={'Description'} />
      <div className="p-4">{book.entry.description}</div>
      <EntrySectionHeader title={'Authors'} />
    </>
  )
}

export const Route = createFileRoute('/entries/books/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(bookQueryOptions(id))
  },

  component: BookEntryComponent,
})
