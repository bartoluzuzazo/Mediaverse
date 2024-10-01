import { FunctionComponent } from 'react'
import { CustomCarousel } from '../../shared/CustomCarousel'
import { useQuery } from '@tanstack/react-query'
import { userService } from '../../../../services/userService.ts'
import CustomImage from '../../customImage'
import { Link } from '@tanstack/react-router'
import SectionHeader from '../../entries/sectionHeader.tsx'

type Props = {
  userId: string
}
export const FriendCarousel: FunctionComponent<Props> = ({ userId }) => {
  const { data: friends } = useQuery({
    queryKey: ['GET_FRIENDS', userId],
    queryFn: async () => {
      return (await userService.getFriends(userId)).data
    },
  })

  if (!friends || friends.length === 0) {
    return null
  }

  return (
    <div>
      <SectionHeader title="Friends" />
      <CustomCarousel>
        {friends.map((f) => {
          return (
            <Link
              to="/users/$id"
              params={{ id: f.id }}
              key={f.id}
              className="m-1 my-5 block w-[150px]"
            >
              <CustomImage
                className="aspect-square h-full w-full rounded-full object-cover"
                src={`data:image/webp;base64,${f.profilePicture}`}
              />
              <div className="text-center text-lg font-semibold text-violet-700">
                {f.username}
              </div>
            </Link>
          )
        })}
      </CustomCarousel>
    </div>
  )
}
