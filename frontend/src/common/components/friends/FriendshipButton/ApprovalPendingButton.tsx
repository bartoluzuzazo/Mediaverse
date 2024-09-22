import { FriendshipButtonLayout } from './FriendshipButtonLayout.tsx'
import { FaClock } from 'react-icons/fa'

export const ApprovalPendingButton = () => {
  return (
    <FriendshipButtonLayout
      icon={<FaClock />}
      onClick={() => {}}
      text={'Approval pending'}
      isSecondary
    />
  )
}
