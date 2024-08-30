import { createFileRoute } from '@tanstack/react-router'
import AuthorForm from '../../../common/components/authors/AuthorForm.tsx'

export const Route = createFileRoute('/authors/create/')({
  component: () => {
    return <AuthorForm />
  },
})
