import { Dispatch, FunctionComponent, SetStateAction, useState } from 'react'
import { useInfiniteQuery } from '@tanstack/react-query'
import { FaSearch } from 'react-icons/fa'
import { useDebounceValue } from 'usehooks-ts'
import { AxiosResponse } from 'axios'
import FormInput from '../form/input'
import { Page, PaginateRequest } from '../../../models/common'

type Props = {
  onClick?: (genre: string) => void | Promise<void>
  searchFunction: (query: string, params: PaginateRequest) => Promise<AxiosResponse<Page<string>>>
  queryKey: string
  query : string
  setQuery :  Dispatch<SetStateAction<string>>
}

export const GenrePicker: FunctionComponent<Props> = ({ onClick, searchFunction, queryKey, query, setQuery }) => {
  const [] = useState<string>('')
  const [debouncedQuery] = useDebounceValue(query, 300)

  const { data: genres, fetchNextPage, isFetchingNextPage } = useInfiniteQuery({
    queryKey: [queryKey, { query: debouncedQuery }],
    queryFn: async ({ pageParam }) => {
      return (await searchFunction(debouncedQuery, { page: pageParam, size: 2 })).data
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
    enabled: !!debouncedQuery,
  })

  const isDone = genres?.pages.some(p => !p.hasNext)

  return (
    <div className="w-full max-w-[800px] mx-auto">
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'search...',
          value: query,
          onChange: (e: { target: { value: SetStateAction<string> } }) => setQuery(e.target.value),
        }}
        rightElement={<FaSearch />}
      />
      <div className='flex flex-wrap gap-6 mt-10 overflow-y-auto'>
        {genres?.pages.flatMap(p => p.contents).map(g => (
          <GenreLinkWrapper key={g} genre={g} onClick={onClick}>
            <GenreTile genre={g} />
          </GenreLinkWrapper>),
        )}
        {!isDone && query && genres && <button type='button'
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
  onClick?: (genre: string) => void | Promise<void>
  children: React.ReactNode
  genre: string
}

const GenreLinkWrapper: FunctionComponent<WrapperProps> = ({ onClick, children, genre }) => {
  if (onClick) {
    return (
      <button onClick={() => onClick(genre)} className="bg-transparent p-0 border-none outline-none">
        {children}
      </button>
    )
  }
  return children
}

type GenreTileProps = {
  genre: string
}

const GenreTile: FunctionComponent<GenreTileProps> = ({ genre }) => {
  return (
    <div className='max-w-[125px]'>
      <div className='text-center text-lg font-semibold text-violet-700'>
        {genre}
      </div>
    </div>
  )
}