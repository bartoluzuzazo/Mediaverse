import { AiFillStar, AiOutlineStar } from 'react-icons/ai'
import { useState } from 'react'

type Props = {
  onClick: (grade: number) => Promise<void> | void
  previousGrade?: number
  max?: number
}
const RatingPicker = ({ onClick, previousGrade, max = 10 }: Props) => {
  const [newRating, setNewRating] = useState<null | number>(null)

  const displayedRating = newRating || previousGrade
  const availableGrades = [...Array(max + 1).keys()].slice(1)

  return (
    <div className="mb-4 mt-10 flex flex-col gap-4 text-xl font-bold md:flex-row md:gap-8">
      <div>Rate it:</div>
      <div
        className="flex text-3xl text-violet-900 hover:text-violet-700 min-[360px]:text-4xl"
        onMouseLeave={() => setNewRating(null)}
      >
        {availableGrades.map((num) => {
          return (
            <div
              key={num}
              onMouseEnter={() => setNewRating(num)}
              onClick={() => onClick(num)}
            >
              {displayedRating && num <= displayedRating ? (
                <AiFillStar />
              ) : (
                <AiOutlineStar />
              )}
            </div>
          )
        })}
      </div>
      <div className="md:ml-auto">
        {displayedRating
          ? `Rating: ${displayedRating}/${max}`
          : 'No rating yet'}
      </div>
    </div>
  )
}
export default RatingPicker
