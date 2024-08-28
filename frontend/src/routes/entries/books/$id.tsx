import { createFileRoute } from '@tanstack/react-router'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import { Book } from '../../../models/entry/book/Book.ts'
import EntrySectionHeader from '../../../common/components/entries/entrySectionHeader.tsx'
import { bookService } from '../../../services/bookService.ts'

export const Route = createFileRoute('/entries/books/$id')({
  loader: async ({ params }) => {
    const response = await bookService.getBook(params.id)
    return response.data
  },

  component: () => {
    const book = Route.useLoaderData<Book>()
    const info = [book.entry.release.toString(), ...book.bookGenres]
    return (
      <>
        <EntryBanner entry={book.entry} info={info} type={'Book'} />
        <EntrySectionHeader title={'Description'} />
        <div className="p-4">
          {book.entry.description}
        </div>
        <EntrySectionHeader title={'Authors'} />
      </>
    )
  },
})