import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { userService } from '../../../../services/userService.ts'
import { CustomCarousel } from '../../shared/CustomCarousel'
import { User } from '../../../../models/user'
import { FriendApprovalComponent } from './FriendApprovalComponent.tsx'
import { useQuery } from '@tanstack/react-query'
import SectionHeader from '../../entries/sectionHeader.tsx'

export const FriendRequestCarousel = () => {
  const { authUserData } = useAuthContext()!
  const { data: requests } = useQuery({
    queryKey: ['GET_FRIEND_REQUESTS', authUserData!.id],
    queryFn: async () => {
      return (await userService.getFriendInvites()).data
    },
  })

  if (!requests || requests.length === 0) {
    return null
  }
  return (
    <div>
      <SectionHeader title="Friend requests" />
      <CustomCarousel>
        {requests.map((f: User) => {
          return (
            <div key={f.id} className="m-1 my-5 block w-[150px]">
              <FriendApprovalComponent friend={f} />
            </div>
          )
        })}
      </CustomCarousel>
    </div>
  )
}
