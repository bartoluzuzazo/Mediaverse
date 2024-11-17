import { FunctionComponent } from 'react'
import { AmaStatus } from '../../../../models/amaSessions'
import { useMutation } from '@tanstack/react-query'
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
  console.log({ amaSessionStatus })
  const navigate = useNavigate()
  const { mutateAsync: endSessionMutation } = useMutation({
    mutationFn: async () => {
      return await amaSessionService.endSession(amaSessionId)
    },
    onSuccess: async () => {
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
      End Session
    </button>
  )
}
