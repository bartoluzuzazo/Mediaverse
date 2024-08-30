import {
  QueryObserverResult,
  RefetchOptions,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query'
import { Entry } from '../../../models/entry/Entry.ts'
import { ratingService } from '../../../services/ratingService.ts'
import axios from 'axios'
import { Rating } from '../../../models/entry/rating/Rating.ts'
import RatingPicker from './RatingPicker.tsx'

interface WithEntry {
  entry: Entry
}

type Props<T extends WithEntry> = {
  entryId: string
  refetch: (options?: RefetchOptions) => Promise<QueryObserverResult<T, Error>>
}

const EntryRatingPicker = <T extends WithEntry>({ entryId }: Props<T>) => {
  const { data: usersRating, isLoading } = useQuery({
    queryKey: ['GET_USERS_RATING'],
    queryFn: async () => {
      try {
        const response = await ratingService.getRating(entryId)
        return response.data
      } catch (error: unknown) {
        if (!axios.isAxiosError(error) || error.response?.status != 404) {
          throw error
        }
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

  return <RatingPicker onClick={onClick} previousGrade={usersRating?.grade} />
}

export default EntryRatingPicker
