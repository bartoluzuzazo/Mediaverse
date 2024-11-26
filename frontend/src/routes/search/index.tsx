import { useInfiniteQuery } from '@tanstack/react-query'
import { createFileRoute, Link } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import { EntryService } from '../../services/entryService'
import InfiniteScroll from 'react-infinite-scroll-component'
import { Oval } from 'react-loader-spinner'
import React from 'react'
import { Entry } from '../../models/entry/Entry'
import CustomImage from '../../common/components/customImage'
import { AiFillStar } from 'react-icons/ai'
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs'
import './styles.css'

type SearchQueryParams = {
  searchQuery: string
}

interface SearchPageProps {}

interface Tab {
  label: string
  value: string
}

const SearchPage: FunctionComponent<SearchPageProps> = () => {
  const { searchQuery } = Route.useSearch()

  return (
    <Tabs className="search">
      <div className="my-5">
        <h1 className="text-xl font-semibold tracking-tight">
          Search reults for query: {searchQuery}
        </h1>
      </div>
      <TabList>
        <Tab>Movies</Tab>
        <Tab>Series</Tab>
        <Tab>Books</Tab>
        <Tab>Games</Tab>
        <Tab>Music</Tab>
      </TabList>
      <TabPanel>
        <SearchTab type={'Movie'} />
      </TabPanel>
      <TabPanel>
        <SearchTab type={'Serie'} />
      </TabPanel>
      <TabPanel>
        <SearchTab type={'Book'} />
      </TabPanel>
      <TabPanel>
        <SearchTab type={'Game'} />
      </TabPanel>
      <TabPanel>
        <SearchTab type={'Music'} />
      </TabPanel>
    </Tabs>
  )
}

interface SearchTabProps {
  type: string
}

const SearchTab: FunctionComponent<SearchTabProps> = ({ type }) => {
  const { searchQuery } = Route.useSearch()

  const { data, fetchNextPage, isFetching } = useInfiniteQuery({
    queryKey: ['search', 'entries', searchQuery, type],
    queryFn: async ({ pageParam }) =>
      await EntryService.getEntries(searchQuery, pageParam, type),
    getNextPageParam: (_, __, lastPageParam) => lastPageParam + 1,
    initialPageParam: 0,
    gcTime: 99999,
  })

  return (
    <InfiniteScroll
      dataLength={(data?.pages.length ?? 0) * 5}
      next={() => !isFetching && fetchNextPage()}
      hasMore={data?.pages.at(-1)?.data?.entries.length !== 0}
      loader={
        <div className="pointer-events-none fixed bottom-0 left-0 right-0 top-0 flex items-center justify-center">
          <Oval
            visible={true}
            height="80"
            width="80"
            color="#5b21b6"
            secondaryColor=""
            ariaLabel="oval-loading"
            wrapperStyle={{}}
            wrapperClass=""
          />
        </div>
      }
    >
      {(data?.pages.at(0)?.data?.entries?.length || 0) > 1 ? (
        data?.pages.map((group, i) => (
          <React.Fragment key={i}>
            {group?.data?.entries.map((project) => (
              <EntryComponent key={project.id} {...project} />
            ))}
          </React.Fragment>
        ))
      ) : (
        <>Brak wyników</>
      )}
    </InfiniteScroll>
  )
}

const EntryComponent: FunctionComponent<Entry> = ({
  id,
  photo,
  name,
  ratingAvg,
  description,
  release,
}) => {
  return (
    <Link
      to="/entries/books/$id"
      className="group flex h-[170px] gap-10 p-3 font-bold text-black"
      params={{ id }}
    >
      <CustomImage
        className="w-[100px] object-cover transition-all group-hover:scale-[1.1]"
        src={`data:image/webp;base64,${photo}`}
      />
      <div className="flex flex-col justify-between">
        <p className="text-3xl font-semibold">{name}</p>
        <div className="flex w-full gap-10 text-lg font-medium">
          <div>{release}</div>
          <div className="flex items-center gap-1">
            {ratingAvg}
            <AiFillStar />
          </div>
        </div>
      </div>
      <p className="line-clamp-6 max-h-[170px] max-w-[500px] grow overflow-hidden text-ellipsis font-normal">
        {description}
      </p>
    </Link>
  )
}

export const Route = createFileRoute('/search/')({
  validateSearch: (search: Record<string, unknown>): SearchQueryParams => {
    return { searchQuery: search.searchQuery + '' }
  },
  component: SearchPage,
})
