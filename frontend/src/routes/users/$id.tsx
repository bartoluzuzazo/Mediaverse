import { createFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import { User } from '../../models/user'
import { userService } from '../../services/userService.ts'

interface Props {}

const UserComponent: FunctionComponent<Props> = () => {
  const user = Route.useLoaderData<User>()
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
          <div className="text-xl font-bold">{user.username}</div>
        </div>

        <div className="flex-1 md:ml-20"></div>
      </div>
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
