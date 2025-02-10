import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { ratingService } from '../../../services/ratingService.ts'
import { Rating } from '../../../models/entry/rating/Rating.ts'
import RatingPicker from './RatingPicker.tsx'
import { serviceUtils } from '../../../services/serviceUtils.ts'

type Props = {
  entryId: string
}

const EntryRatingPicker = ({ entryId }: Props) => {
  const { data: usersRating, isLoading } = useQuery({
    queryKey: ['GET_USERS_RATING', entryId],
    queryFn: async () => {
      const getFn = () => ratingService.getRating(entryId)
      return await serviceUtils.getIfFound(getFn)
    },
    select: (data) => {
      return data || { entryId }
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
      queryClient.invalidateQueries({ queryKey: ['GET_USERS_RATING', entryId] })
      queryClient.invalidateQueries({ queryKey: ['GET_ENTRY', entryId] })
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
