import { FunctionComponent, useState } from 'react'
import { User } from '../../../../models/user'
import { Link } from '@tanstack/react-router'
import CustomImage from '../../customImage'
import { useInfiniteQuery } from '@tanstack/react-query'
import FormInput from '../../form/input'
import { FaSearch } from 'react-icons/fa'
import { useDebounceValue } from 'usehooks-ts'
import { Author } from '../../../../models/author/Author.ts'
import { Page, PaginateRequest } from '../../../../models/common'
import { AxiosResponse } from 'axios'

type Props = {
  onClick?: (user: User | Author) => void | Promise<void>
  searchFunction : (query: string, params: PaginateRequest) => Promise<AxiosResponse<Page<User | Author>>>
  queryKey: string
}

export const UserSearch: FunctionComponent<Props> = ({ onClick, searchFunction, queryKey }) => {
  const [query, setQuery] = useState<string>('')
  const [debouncedQuery] = useDebounceValue(query, 300)

  const { data: users, fetchNextPage, isFetchingNextPage } = useInfiniteQuery({
    queryKey: [queryKey, { query: debouncedQuery }],
    queryFn: async ({ pageParam }) => {
      return (await searchFunction(debouncedQuery, { page: pageParam, size: 2 })).data
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
    enabled: !!debouncedQuery,
  })

  const isDone = users?.pages.some(p => !p.hasNext)

  return (
    <div className='w-full max-w-[800px] mx-auto'>
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'search...',
          value: query,
          onChange: (e) => setQuery(e.target.value),
        }}
        rightElement={<FaSearch />}
      />
      <div className='flex flex-wrap gap-6 mt-10 overflow-y-auto'>
        {users?.pages.flatMap(p => p.contents).map(u => (
          <UserLinkWrapper key={u.id} user={u} onClick={onClick}>
            <UserTile user={u} />
          </UserLinkWrapper>),
        )}
        {!isDone && query && users && <button type='button'
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
  onClick?: (user: User | Author) => void | Promise<void>
  children: React.ReactNode
  user: User | Author
}

const UserLinkWrapper: FunctionComponent<WrapperProps> = ({ onClick, children, user }) => {
  if (onClick) {
    return (
      <button onClick={() => onClick(user)} className="bg-transparent p-0 border-none outline-none">
        {children}
      </button>
    )
  }
  return (
    <Link to={'username' in user ? '/users/$id' : '/authors/$id'} params={{ id: user.id }}>
      {children}
    </Link>
  )
}

type UserTileProps = {
  user: User | Author
}

const UserTile: FunctionComponent<UserTileProps> = ({ user }) => {
  return (
    <div className='max-w-[125px]'>
      <CustomImage
        className='aspect-square h-full w-full rounded-full object-cover'
        src={`data:image/webp;base64,${user.profilePicture}`}
      />
      <div className='text-center text-lg font-semibold text-violet-700'>
        {'username' in user ? user.username : `${user.name} ${user.surname}`}
      </div>
    </div>
  )
}