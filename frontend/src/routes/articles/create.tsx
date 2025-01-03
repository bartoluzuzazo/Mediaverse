import { createFileRoute, Navigate } from '@tanstack/react-router'
import { ArticleForm } from '../../common/components/articles/articleForm'
import { AuthorizedView } from '../../common/components/auth/AuthorizedView'

export const Route = createFileRoute('/articles/create')({
  component: () => <CreateArticle />,
})
const CreateArticle = () => {
  return (
    <AuthorizedView
      notAuthView={<Navigate to={'/'} />}
      allowedRoles="ContentCreator"
    >
      <ArticleForm />
    </AuthorizedView>
  )
}
