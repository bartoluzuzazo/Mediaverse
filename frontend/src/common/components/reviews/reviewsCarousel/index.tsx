import { FunctionComponent } from 'react'
import { CustomCarousel } from '../../shared/CustomCarousel'
import { reviewService } from '../../../../services/reviewService.ts'
import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import CustomImage from '../../customImage'
import SectionHeader from '../../entries/sectionHeader.tsx'
import { RatingDisplay } from '../ratingDisplay'
import { YourReviewLink } from './yourReviewLink.tsx'

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

  if (!reviewSummary || reviewSummary.reviews.length === 0) {
    return null
  }
  const reviews = reviewSummary.reviews

  return (
    <div>
      <SectionHeader title="Reviews" />
      <p className="mt-2 text-xl font-semibold">
        {' '}
        Average rating: {reviewSummary.gradeAvg}
      </p>
      <YourReviewLink reviews={reviews} entryId={entryId} />

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
              <div className="italic">{review.title}</div>
            </Link>
          )
        })}
      </CustomCarousel>
    </div>
  )
}
