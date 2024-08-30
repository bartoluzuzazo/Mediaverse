import { AiFillStar, AiOutlineStar } from 'react-icons/ai'
import { useState } from 'react'

type Props = {
  onClick: (grade: number) => Promise<void>
  previousGrade?: number
}
const RatingPicker = ({ onClick, previousGrade }: Props) => {
  const [newRating, setNewRating] = useState<null | number>(null)

  const displayedRating = newRating || previousGrade
  const avaiableGrades = [...Array(11).keys()].slice(1)

  return (
    <div className="mb-4 mt-10 flex flex-col gap-4 text-xl font-bold md:flex-row md:gap-8">
      <div>Rate it:</div>
      <div
        className="flex text-3xl text-violet-900 hover:text-violet-700 min-[360px]:text-4xl"
        onMouseLeave={() => setNewRating(null)}
      >
        {avaiableGrades.map((num) => {
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
        {displayedRating ? `Rating: ${displayedRating}/10` : 'No rating yet'}
      </div>
    </div>
  )
}
export default RatingPicker
