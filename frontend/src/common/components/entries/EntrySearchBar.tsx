import { FunctionComponent, useState } from 'react'
import { Link } from '@tanstack/react-router'
import { useInfiniteQuery } from '@tanstack/react-query'
import { FaSearch } from 'react-icons/fa'
import { useDebounceValue } from 'usehooks-ts'
import { AxiosResponse } from 'axios'
import { EntrySearch } from '../../../models/entry/Entry.ts'
import { Page, PaginateRequest } from '../../../models/common'
import FormInput from '../form/input'
import CustomImage from '../customImage'

type Props = {
  onClick?: (entry: EntrySearch) => void | Promise<void>
  searchFunction : (query: string, params: PaginateRequest) => Promise<AxiosResponse<Page<EntrySearch>>>
  queryKey: string
}

export const EntrySearchBar: FunctionComponent<Props> = ({ onClick, searchFunction, queryKey }) => {
  const [query, setQuery] = useState<string>('')
  const [debouncedQuery] = useDebounceValue(query, 300)

  const { data: entrys, fetchNextPage, isFetchingNextPage } = useInfiniteQuery({
    queryKey: [queryKey, { query: debouncedQuery }],
    queryFn: async ({ pageParam }) => {
      return (await searchFunction(debouncedQuery, { page: pageParam, size: 2 })).data
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
    enabled: !!debouncedQuery,
  })

  const isDone = entrys?.pages.some(p => !p.hasNext)

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
        {entrys?.pages.flatMap(p => p.contents).map(u => (
          <UserLinkWrapper key={u.id} entry={u} onClick={onClick}>
            <UserTile entry={u} />
          </UserLinkWrapper>),
        )}
        {!isDone && query && entrys && <button type='button'
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
  onClick?: (entry: EntrySearch) => void | Promise<void>
  children: React.ReactNode
  entry: EntrySearch
}

const UserLinkWrapper: FunctionComponent<WrapperProps> = ({ onClick, children, entry }) => {
  if (onClick) {
    return (
      <button onClick={() => onClick(entry)} className="bg-transparent p-0 border-none outline-none">
        {children}
      </button>
    )
  }
  return (
    <Link to='/entries/$id' params={{ id: entry.id }}>
      {children}
    </Link>
  )
}

type UserTileProps = {
  entry: EntrySearch
}

const UserTile: FunctionComponent<UserTileProps> = ({ entry }) => {
  return (
    <div className='max-w-[125px]'>
      <CustomImage
        className='aspect-square h-full w-full object-cover'
        src={`data:image/webp;base64,${entry.photo}`}
      />
      <div className='text-center text-lg font-semibold text-violet-700'>
        {entry.name}
      </div>
    </div>
  )
}