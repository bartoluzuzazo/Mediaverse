import { FriendshipButtonLayout } from './FriendshipButtonLayout.tsx'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { FunctionComponent } from 'react'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { IoPersonAddSharp } from 'react-icons/io5'

type Props = {
  friendId: string
}
export const InviteFriendButton: FunctionComponent<Props> = ({ friendId }) => {
  const queryClient = useQueryClient()
  const { mutateAsync: inviteFriendMutationAsync } = useMutation({
    mutationFn: friendshipService.postInvitation,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['GET_FRIENDSHIP', friendId] })
    },
  })
  const onClick = async () => {
    await inviteFriendMutationAsync(friendId)
  }
  return (
    <FriendshipButtonLayout
      icon={<IoPersonAddSharp />}
      onClick={onClick}
      text="Invite friend"
    />
  )
}
