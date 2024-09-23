import { FriendshipButtonBase } from './FriendshipButtonBase.tsx'
import { FunctionComponent } from 'react'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { IoPersonAddSharp } from 'react-icons/io5'

type Props = {
  friendId: string
}
export const InviteFriendButton: FunctionComponent<Props> = ({ friendId }) => {
  return (
    <FriendshipButtonBase
      icon={<IoPersonAddSharp />}
      text="Send friend request"
      friendId={friendId}
      mutationFn={friendshipService.postInvitation}
    />
  )
}
