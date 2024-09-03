import { createFileRoute } from '@tanstack/react-router'
import BookForm from '../../../../common/components/entries/books/bookForm.tsx'

export const Route = createFileRoute('/entries/books/create/')({
  component: () => <BookForm/>
})