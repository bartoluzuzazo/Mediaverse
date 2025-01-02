import { FunctionComponent } from 'react'
import { CustomCarousel } from '../../shared/CustomCarousel'
import { reviewService } from '../../../../services/reviewService.ts'
import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import CustomImage from '../../customImage'
import SectionHeader from '../../entries/sectionHeader.tsx'
import { RatingDisplay } from '../ratingDisplay'
import { YourReviewLink } from './yourReviewLink.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'

type Props = {
  entryId: string
}

export const ReviewsCarousel: FunctionComponent<Props> = ({ entryId }) => {
  const { data: reviewSummary } = useQuery({
    queryKey: ['GET_REVIEWS'],
    queryFn: async () => {
      return (await reviewService.getReviews(entryId)).data
    },
  })

  if (!reviewSummary) {
    return null
  }
  if (reviewSummary.reviews.length === 0) {
    return (
      <AuthorizedView allowedRoles="Critic">
        <YourReviewLink reviews={reviewSummary.reviews} entryId={entryId} />
      </AuthorizedView>
    )
  }
  const reviews = reviewSummary.reviews

  return (
    <div className="space-y-2">
      <SectionHeader title="Reviews" />
      <p className="mt-2 text-xl font-semibold">
        {' '}
        Average rating: {reviewSummary.gradeAvg}
      </p>
      <AuthorizedView allowedRoles="Critic">
        <YourReviewLink reviews={reviews} entryId={entryId} />
      </AuthorizedView>

      <CustomCarousel>
        {reviews.map((review) => {
          return (
            <Link
              to="/reviews/$userId/entries/$entryId"
              params={{ userId: review.userId, entryId: entryId }}
              key={review.userId + review.entryId}
              className="m-1 my-5 block w-[150px]"
            >
              <CustomImage
                className="aspect-square h-full w-full rounded-full object-cover"
                src={`data:image/webp;base64,${review.profilePicture}`}
              />
              <div className="text-center text-lg font-semibold text-violet-700">
                {review.username}
              </div>
              <RatingDisplay rating={review.grade} />
              <div className="italic text-slate-500">{review.title}</div>
            </Link>
          )
        })}
      </CustomCarousel>
    </div>
  )
}
