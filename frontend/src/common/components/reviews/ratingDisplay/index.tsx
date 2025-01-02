import { AiFillStar, AiOutlineStar } from 'react-icons/ai'

interface RatingDisplayProps {
  rating: number
}

export const RatingDisplay = ({ rating }: RatingDisplayProps) => {
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
