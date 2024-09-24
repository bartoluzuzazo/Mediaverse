import { createFileRoute } from '@tanstack/react-router'
import BookForm from '../../../../common/components/entries/books/BookForm.tsx'

export const Route = createFileRoute('/entries/books/create/')({
  component: () => <BookForm/>
})