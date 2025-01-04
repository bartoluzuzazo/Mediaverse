import { createFileRoute, Navigate } from '@tanstack/react-router'
import { AuthorizedView } from '../../../common/components/auth/AuthorizedView'
import { ReviewForm } from '../../../common/components/reviews/reviewForm'

export const Route = createFileRoute('/entries/create-review/$id')({
  component: () => <CreateReviewComponent />,
})
const CreateReviewComponent = () => {
  const entryId = Route.useParams().id
  return (
    <AuthorizedView notAuthView={<Navigate to="/" />} allowedRoles="Critic">
      <ReviewForm entryId={entryId} />
    </AuthorizedView>
  )
}
