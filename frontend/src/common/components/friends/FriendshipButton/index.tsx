import { useQuery } from '@tanstack/react-query'
import { FunctionComponent } from 'react'
import { serviceUtils } from '../../../../services/serviceUtils.ts'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { InviteFriendButton } from './InviteFriendButton.tsx'
import { RemoveFriendButton } from './RemoveFriendButton.tsx'
import { ApproveFriendButton } from './ApproveFriendshipButton.tsx'
import { FaCircleXmark } from 'react-icons/fa6'
import { MdCancelScheduleSend, MdPersonRemove } from 'react-icons/md'

type Props = {
  friendId: string
}
export const FriendshipButton: FunctionComponent<Props> = ({ friendId }) => {
  const { data: friendship, isLoading } = useQuery({
    queryKey: ['GET_FRIENDSHIP', friendId],
    queryFn: async () => {
      const getFn = () => friendshipService.getFriendship(friendId)
      return await serviceUtils.getIfFound(getFn)
    },
  })
  const { userData } = useAuthContext()!
  if (isLoading || !userData || friendId === userData.id) {
    return null
  }
  if (!friendship) {
    return <InviteFriendButton friendId={friendId} />
  }
  if (friendship.approved) {
    return (
      <RemoveFriendButton
        friendId={friendId}
        text={'Remove friend'}
        icon={<MdPersonRemove />}
      />
    )
  }
  const currentUserIsOferrer = friendship.userId === userData.id

  if (currentUserIsOferrer) {
    return (
      <RemoveFriendButton
        friendId={friendId}
        text={'Rescind request'}
        icon={<MdCancelScheduleSend />}
      />
    )
  }
  const currentUserIsRecipient = friendship.user2Id === userData.id
  if (currentUserIsRecipient) {
    return (
      <div>
        <ApproveFriendButton friendId={friendId} />
        <RemoveFriendButton
          friendId={friendId}
          text="Reject request"
          icon={<FaCircleXmark />}
        />
      </div>
    )
  }
}
