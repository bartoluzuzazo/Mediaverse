import { User } from '../../../../models/user'
import { FunctionComponent } from 'react'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import CustomImage from '../../customImage'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { friendshipService } from '../../../../services/friendshipService.ts'
import { Link } from '@tanstack/react-router'

type Props = {
  friend: User
}
export const FriendApprovalComponent: FunctionComponent<Props> = ({
  friend,
}) => {
  const { authUserData } = useAuthContext()!
  const queryClient = useQueryClient()
  const friendRequestKey = ['GET_FRIEND_REQUESTS', authUserData!.id]
  const friendsKey = ['GET_FRIENDS', authUserData!.id]
  const { mutateAsync: approveAsync } = useMutation({
    mutationFn: friendshipService.postFriendshipApproval,
    onMutate: (_friendId) => {
      const prevRequests = queryClient.getQueryData<User[]>(friendRequestKey)
      const prevFriends = queryClient.getQueryData<User[]>(friendsKey)
      queryClient.setQueryData(friendRequestKey, (data: User[]) =>
        data.filter((u) => u.id !== friend.id)
      )
      queryClient.setQueryData(friendsKey, (data: User[]) =>
        data.concat(friend)
      )
      return { prevRequests, prevFriends }
    },
    onError: (_err, _friendId, context) => {
      queryClient.setQueryData(friendsKey, context!.prevFriends)
      queryClient.setQueryData(friendRequestKey, context!.prevRequests)
    },
  })

  const { mutateAsync: rejectAsync } = useMutation({
    mutationFn: friendshipService.deleteFriendship,
    onMutate: (_friendId) => {
      const prevRequests = queryClient.getQueryData<User[]>(friendRequestKey)
      queryClient.setQueryData(friendRequestKey, (data: User[]) =>
        data.filter((u) => u.id !== friend.id)
      )
      return { prevRequests }
    },
    onError: (_err, _friendId, context) => {
      queryClient.setQueryData(friendRequestKey, context!.prevRequests)
    },
  })
  return (
    <div className="m-1 my-5 block w-[150px]">
      <Link to="/users/$id" params={{ id: friend.id }} key={friend.id}>
        <CustomImage
          className="aspect-square h-full w-full rounded-full"
          src={`data:image/webp;base64,${friend.profilePicture}`}
        />
        <div className="text-center text-lg font-semibold text-violet-700">
          {friend.username}
        </div>
      </Link>
      <div className="flex">
        <button
          type="button"
          className="flex-1 basis-0 gap-2 bg-slate-950 text-white"
          onClick={async () => {
            await approveAsync(friend.id)
          }}
        >
          Approve
        </button>
        <button
          type="button"
          className="flex-1 basis-0 gap-2 bg-slate-300 text-slate-950"
          onClick={async () => {
            await rejectAsync(friend.id)
          }}
        >
          Reject
        </button>
      </div>
    </div>
  )
}
