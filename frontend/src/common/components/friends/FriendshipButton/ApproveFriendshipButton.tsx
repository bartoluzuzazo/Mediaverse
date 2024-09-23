import { FriendshipButtonBase } from './FriendshipButtonBase.tsx'
import { FunctionComponent } from 'react'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { FaCircleCheck } from 'react-icons/fa6'

type Props = {
  friendId: string
}
export const ApproveFriendButton: FunctionComponent<Props> = ({ friendId }) => {
  return (
    <FriendshipButtonBase
      icon={<FaCircleCheck />}
      text="Approve request"
      mutationFn={friendshipService.postFriendshipApproval}
      friendId={friendId}
    />
  )
}
