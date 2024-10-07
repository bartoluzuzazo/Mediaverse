import { FunctionComponent, useState } from 'react'
import { User } from '../../../../models/user'
import { Link } from '@tanstack/react-router'
import CustomImage from '../../customImage'
import { useInfiniteQuery } from '@tanstack/react-query'
import { userService } from '../../../../services/userService.ts'
import FormInput from '../../form/input'
import { FaSearch } from 'react-icons/fa'
import { useDebounceValue } from 'usehooks-ts'

type Props = {
  onClick?: (user: User) => void | Promise<void>
}

export const UserSearch: FunctionComponent<Props> = ({ onClick }) => {
  const [query, setQuery] = useState<string>('')
  const [debouncedQuery]=useDebounceValue(query, 1000)

  const { data: users, fetchNextPage, isFetchingNextPage } = useInfiniteQuery({
    queryKey: ['SEARCH_USER', { query: debouncedQuery }],
    queryFn: async ({ pageParam }) => {
      return (await userService.searchUsers(debouncedQuery, { page: pageParam, size: 2 })).data
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
    enabled: !!debouncedQuery,
  })

  const isDone = users?.pages.some(p => !p.hasNext)
  console.log(isFetchingNextPage)

  return (
    <div className='max-w-[800px] mx-auto'>
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'search...',
          value: query,
          onChange: (e) => setQuery(e.target.value),
        }}
        rightElement={<FaSearch />}
      />
      <div className='flex flex-wrap gap-6 mt-10'>
        {users?.pages.flatMap(p => p.contents).map(u => (
          <UserLinkWrapper key={u.id} user={u} onClick={onClick}>
            <UserTile user={u} />
          </UserLinkWrapper>),
        )}
        {!isDone && query && <button type='button'
                                     disabled={isFetchingNextPage}
                                     onClick={() => fetchNextPage()}
                                     className={` w-[125px] aspect-square  rounded-full text-white self-start ${isFetchingNextPage ? 'bg-violet-400' : 'bg-violet-900 hover:bg-violet-800'}`}>
          <span className="font-extrabold text-white rotate-12 block text-xl capitalize">next</span>
        </button>}
      </div>



    </div>

  )
}
type WrapperProps = {
  onClick?: (user: User) => void | Promise<void>
  children: React.ReactNode
  user: User
}
const UserLinkWrapper: FunctionComponent<WrapperProps> = ({ onClick, children, user }) => {
  if (onClick) {
    return (
      <button onClick={() => onClick(user)}>
        {children}
      </button>
    )
  }

  return (
    <Link to='/users/$id' params={{ id: user.id }}>
      {children}
    </Link>
  )

}

type UserTileProps = {
  user: User
}

const UserTile: FunctionComponent<UserTileProps> = ({ user }) => {
  return (
    <div className='max-w-[125px]'>
      <CustomImage
        className='aspect-square h-full w-full rounded-full object-cover'
        src={`data:image/webp;base64,${user.profilePicture}`}
      />
      <div className='text-center text-lg font-semibold text-violet-700'>
        {user.username}
      </div>

    </div>
  )
}