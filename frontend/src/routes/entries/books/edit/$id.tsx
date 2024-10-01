import { createFileRoute } from '@tanstack/react-router'
import { BookService } from '../../../../services/bookService.ts'
import { Book } from '../../../../models/entry/book/Book.ts'
import BookForm from '../../../../common/components/entries/books/BookForm.tsx'

export const Route = createFileRoute('/entries/books/edit/$id')({
  loader: async ({ params }) => {
    const response = await BookService.getBook(params.id)
    return response.data
  },
  component: () => {
    const book = Route.useLoaderData<Book>()
    return book && <BookForm book={book} />
  },
})
