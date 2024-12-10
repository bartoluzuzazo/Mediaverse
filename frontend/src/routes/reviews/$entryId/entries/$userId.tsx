import { createFileRoute, Link } from '@tanstack/react-router'
import { reviewService } from '../../../../services/reviewService.ts'
import { FunctionComponent } from 'react'
import CustomImage from '../../../../common/components/customImage'
import { AiFillStar, AiOutlineStar } from 'react-icons/ai'
import MarkdownPreview from '@uiw/react-markdown-preview'

export const Route = createFileRoute('/reviews/$entryId/entries/$userId')({
  loader: async ({ params }) => {
    const response = await reviewService.getReview(
      params.entryId,
      params.userId
    )
    return response.data
  },
  component: () => <ReviewComponent />,
})

const ReviewComponent: FunctionComponent = () => {
  const review = Route.useLoaderData()
  return (
    <article className="flex p-2">
      <div className="flex-1">
        <div>
          <h1>{review.title}</h1>
          <p className="mb-6">
            A review of "{review.entryTitle}" by{' '}
            <Link to="/users/$id" params={{ id: review.userId }}>
              {review.username}
            </Link>
          </p>
          <MarkdownPreview
            source={review.content}
            wrapperElement={{ 'data-color-mode': 'light' }}
          />
        </div>
      </div>

      <div className="mb-8 max-w-fit">
        <Link
          className="m-1 my-3 block h-[235px] w-[170px] overflow-hidden transition-shadow"
          to="/entries/$id"
          params={{ id: review.entryId }}
        >
          <CustomImage
            className="h-full w-full object-cover transition-all hover:scale-[1.1]"
            src={`data:image/webp;base64,${review.coverPhoto}`}
          />
        </Link>

        <RatingDisplay rating={review.grade} />
      </div>
    </article>
  )
}

interface RatingDisplayProps {
  rating: number
}

const RatingDisplay = ({ rating }: RatingDisplayProps) => {
  const availableGrades = [...Array(6).keys()].slice(1)
  return (
    <div className="mx-auto flex max-w-fit text-2xl text-black min-[360px]:text-3xl">
      {availableGrades.map((num) => {
        return (
          <div key={num}>
            {num <= rating ? <AiFillStar /> : <AiOutlineStar />}
          </div>
        )
      })}
    </div>
  )
}
