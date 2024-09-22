import { FunctionComponent } from 'react'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { FriendshipButtonLayout } from './FriendshipButtonLayout.tsx'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { IoPersonRemoveSharp } from 'react-icons/io5'

type Props = {
  friendId: string
  text: string
}
export const RemoveFriendButton: FunctionComponent<Props> = ({
  friendId,
  text,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync: removeFriendMutation } = useMutation({
    mutationFn: friendshipService.deleteFriendship,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['GET_FRIENDSHIP', friendId] })
    },
  })
  const onClick = async () => {
    await removeFriendMutation(friendId)
  }
  return (
    <FriendshipButtonLayout
      icon={<IoPersonRemoveSharp />}
      onClick={onClick}
      text={text}
      isSecondary
    />
  )
}
