import {
  QueryObserverResult,
  RefetchOptions,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query'
import { Entry } from '../../../models/entry/Entry.ts'
import { useState } from 'react'
import { ratingService } from '../../../services/ratingService.ts'
import axios, { AxiosError } from 'axios'
import { Rating } from '../../../models/entry/rating/Rating.ts'
import { AiFillStar, AiOutlineStar } from 'react-icons/ai'

interface WithEntry {
  entry: Entry
}

type Props<T extends WithEntry> = {
  entryId: string
  refetch: (options?: RefetchOptions) => Promise<QueryObserverResult<T, Error>>
}

const RatingPicker = <T extends WithEntry>({ entryId }: Props<T>) => {
  const [newRating, setNewRating] = useState<null | number>(null)
  const { data: usersRating, isLoading } = useQuery({
    queryKey: ['GET_USERS_RATING'],
    queryFn: async () => {
      try {
        const response = await ratingService.getRating(entryId)
        return response.data
      } catch (error: unknown | AxiosError) {
        if (!axios.isAxiosError(error) || error.response?.status != 404) {
          console.log('other error')
          throw error
        }
        console.log('our error')
        const rating: Rating = { entryId: entryId }
        return rating
      }
    },
  })
  const queryClient = useQueryClient()

  const { mutateAsync: sendRankingMutation } = useMutation({
    mutationFn: async (updatedRanking: Rating) => {
      if (updatedRanking.id == null) {
        await ratingService.postRating(entryId, updatedRanking)
      } else {
        await ratingService.putRating(updatedRanking.id, updatedRanking)
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['GET_USERS_RATING'] })
      queryClient.invalidateQueries({ queryKey: ['GET_BOOK'] })
    },
  })

  const onClick = async (grade: number) => {
    const updatedRanking: Rating = { ...usersRating!, grade }
    await sendRankingMutation(updatedRanking)
  }

  if (isLoading) {
    return <div>Loading...</div>
  }
  const displayedRating = newRating || usersRating?.grade
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
