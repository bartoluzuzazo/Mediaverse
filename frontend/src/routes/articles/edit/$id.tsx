import { createFileRoute, Navigate } from '@tanstack/react-router'
import { AuthorizedView } from '../../../common/components/auth/AuthorizedView'
import { ArticleForm } from '../../../common/components/articles/articleForm'
import { articleService } from '../../../services/articleService.ts'

export const Route = createFileRoute('/articles/edit/$id')({
  loader: async ({ params }) => {
    const response = await articleService.getArticle(params.id)
    return response.data
  },
  component: () => <EditArticle />,
})

const EditArticle = () => {
  const article = Route.useLoaderData()
  return (
    <AuthorizedView
      notAuthView={<Navigate to={'/'} />}
      allowedRoles="ContentCreator"
    >
      <ArticleForm article={article} />
    </AuthorizedView>
  )
}
