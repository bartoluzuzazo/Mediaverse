import { FunctionComponent, ReactNode } from 'react'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { FriendshipButtonBase } from './FriendshipButtonBase.tsx'

type Props = {
  friendId: string
  text: string
  icon: ReactNode
}
export const RemoveFriendButton: FunctionComponent<Props> = ({
  friendId,
  text,
  icon,
}) => {
  return (
    <FriendshipButtonBase
      mutationFn={friendshipService.deleteFriendship}
      icon={icon}
      text={text}
      isSecondary
      friendId={friendId}
    />
  )
}
