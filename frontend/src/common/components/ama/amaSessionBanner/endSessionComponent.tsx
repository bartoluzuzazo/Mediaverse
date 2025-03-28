import { FunctionComponent } from 'react'
import { AmaStatus } from '../../../../models/amaSessions'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from '@tanstack/react-router'
import { amaSessionService } from '../../../../services/amaSessionService.ts'

type Props = {
  amaSessionStatus: AmaStatus
  amaSessionId: string
  authorId: string
}

export const EndSessionComponent: FunctionComponent<Props> = ({
  amaSessionStatus,
  amaSessionId,
  authorId,
}) => {
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const { mutateAsync: endSessionMutation } = useMutation({
    mutationFn: async () => {
      return await amaSessionService.endSession(amaSessionId)
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['GET_AMA_SESSION', amaSessionId],
      })
      await queryClient.invalidateQueries({
        queryKey: ['GET_AUTHORS_SESSIONS', authorId],
      })
      await navigate({ to: `/authors/$id`, params: { id: authorId } })
    },
  })

  if (
    amaSessionStatus === AmaStatus.Cancelled ||
    amaSessionStatus === AmaStatus.Finished
  ) {
    return null
  }
  return (
    <button
      onClick={async () => {
        await endSessionMutation()
      }}
      className="bg-violet-300 px-1.5 py-0.5 text-black"
    >
      {amaSessionStatus === AmaStatus.Upcoming
        ? 'Cancel session'
        : 'End session'}
    </button>
  )
}
