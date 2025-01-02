import { FunctionComponent } from 'react'
import { ReviewPreview } from '../../../../models/review'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { LinkButton } from '../../shared/LinkButton'
import { AiFillStar } from 'react-icons/ai'

type Props = {
  reviews: ReviewPreview[]
  entryId: string
}

export const YourReviewLink: FunctionComponent<Props> = ({
  reviews,
  entryId,
}) => {
  const { authUserData } = useAuthContext()!
  const userHasReview = reviews.some((r) => r.userId === authUserData?.id)
  if (userHasReview) {
    return (
      <LinkButton
        icon={<AiFillStar />}
        to="/reviews/$userId/entries/edit/$entryId"
        params={{ userId: authUserData!.id, entryId: entryId }}
      >
        Edit review
      </LinkButton>
    )
  }
  return (
    <LinkButton
      icon={<AiFillStar />}
      to="/entries/create-review/$id"
      params={{ id: entryId }}
    >
      Add review
    </LinkButton>
  )
}
