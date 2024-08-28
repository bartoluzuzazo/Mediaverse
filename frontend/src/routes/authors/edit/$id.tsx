import { createFileRoute } from '@tanstack/react-router'
import AuthorForm from '../../../common/components/authors/AuthorForm.tsx'
import { AuthorService } from '../../../services/AuthorService.ts'
import { Author } from '../../../models/author/Author.ts'

export const Route = createFileRoute('/authors/edit/$id')({
  loader: async ({ params }) => {
    const response = await AuthorService.getAuthor(params.id)
    return response.data
  },
  component: () => {
    const author = Route.useLoaderData<Author>()
    return author && <AuthorForm author={author} />
  },
})
