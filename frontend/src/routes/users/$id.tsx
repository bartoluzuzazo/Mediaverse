import { createFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import { User } from '../../models/user'
import { userService } from '../../services/userService.ts'
import { useAuthContext } from '../../context/auth/useAuthContext.ts'
import { FriendshipButton } from '../../common/components/friends/FriendshipButton'
import { FriendCarousel } from '../../common/components/users/FriendCarousel'
import { FriendRequestCarousel } from '../../common/components/users/FriendRequestCarousel'
import { RatedEntryCarousel } from '../../common/components/users/RatedEntryCarousel.tsx'

interface Props {}

const UserComponent: FunctionComponent<Props> = () => {
  const user = Route.useLoaderData<User>()
  const { authUserData } = useAuthContext()!
  const imgSrc = 'data:image/*;base64,' + user.profilePicture
  return (
    <div>
      <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800 md:h-32"></div>
      <div className="flex flex-col p-4 md:flex-row">
        <div>
          <img
            src={imgSrc}
            className="-mt-16 aspect-square w-52 rounded-full border-4 border-white bg-slate-300 object-cover md:-mt-24 md:w-60"
            alt="profile picture"
          />
          <div className="text-3xl font-bold">{user.username}</div>
          {authUserData && authUserData.id !== user.id ? (
            <FriendshipButton friendId={user.id} />
          ) : null}
        </div>

        <div className="flex-1 md:ml-20"></div>
      </div>
      <FriendCarousel userId={user.id} />

      {authUserData && authUserData.id == user.id && (
        <>
          <FriendRequestCarousel />
        </>
      )}
      <RatedEntryCarousel userId={user.id} />
    </div>
  )
}
export const Route = createFileRoute('/users/$id')({
  loader: async ({ params }) => {
    const response = await userService.getUser(params.id)
    return response.data
  },
  component: UserComponent,
})
