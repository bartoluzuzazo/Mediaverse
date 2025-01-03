import { createFileRoute, Navigate } from '@tanstack/react-router'
import { reviewService } from '../../../../../services/reviewService.ts'
import { AuthorizedView } from '../../../../../common/components/auth/AuthorizedView'
import { useAuthContext } from '../../../../../context/auth/useAuthContext.ts'
import { ReviewForm } from '../../../../../common/components/reviews/reviewForm'

export const Route = createFileRoute('/reviews/$userId/entries/edit/$entryId')({
  loader: async ({ params }) => {
    const response = await reviewService.getReview(
      params.entryId,
      params.userId
    )
    return response.data
  },
  component: () => <EditReviewComponent />,
})
const EditReviewComponent = () => {
  const review = Route.useLoaderData()
  const entryId = Route.useParams().entryId
  const { authUserData } = useAuthContext()!
  return (
    <AuthorizedView
      notAuthView={<Navigate to="/" />}
      allowedRoles="Critic"
      requiredUserId={authUserData?.id}
    >
      <ReviewForm entryId={entryId} review={review} />
    </AuthorizedView>
  )
}
