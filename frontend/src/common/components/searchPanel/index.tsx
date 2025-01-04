import { FunctionComponent, useEffect } from 'react'
import { useSearchPanelContext } from '../../../context/searchPanel'
import { Oval } from 'react-loader-spinner'
import CustomImage from '../customImage'
import { AiFillStar } from 'react-icons/ai'
import { Link, useRouterState } from '@tanstack/react-router'
import { IoMdClose } from 'react-icons/io'
import AuthorPreview from '../entries/AuthorPreview.tsx'

interface SearchPanelProps {}

const SearchPanel: FunctionComponent<SearchPanelProps> = () => {
  const searchPanelContext = useSearchPanelContext()
  const router = useRouterState()

  useEffect(() => {
    searchPanelContext?.setSearchValue('')
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [router.location.pathname, searchPanelContext?.setSearchValue])

  if (searchPanelContext?.debounceSearchValue && searchPanelContext.searchValue)
    return (
      <div className="absolute left-0 right-0 top-0 z-50 mt-2 min-h-[400px] w-full rounded-md border border-solid border-mv-gray bg-white p-5 shadow-md">
        <div className="flex flex-col">
          <p className="flex w-full justify-between text-2xl font-semibold">
            <span>Media</span>{' '}
            <IoMdClose
              className="hover:cursor-pointer"
              onClick={() => searchPanelContext.setSearchValue('')}
            />
          </p>
          {searchPanelContext?.searchQuery.isFetching ? (
            <Loader />
          ) : (
            <div className="flex">
              {(searchPanelContext?.searchQuery?.data?.data.entries.length ||
                0) <= 0 ? (
                <div className="flex h-full w-full items-center justify-center text-2xl font-semibold text-mv-gray">
                  No entries
                </div>
              ) : (
                searchPanelContext?.searchQuery?.data?.data.entries.map((e) => (
                  <Link
                    onClick={() => searchPanelContext.setSearchValue('')}
                    to="/entries/$id"
                    key={e.id}
                    className="flex min-h-[300px] w-[200px] flex-col gap-2 p-3 font-bold text-black"
                    params={{ id: e.id }}
                  >
                    <CustomImage
                      className="h-full w-full transition-all hover:scale-[1.1]"
                      src={`data:image/webp;base64,${e.photo}`}
                    />
                    <p className="text-xl">{e.name}</p>
                    <div className="flex w-full justify-between">
                      <div className="flex items-center gap-1">
                        {e.ratingAvg.toFixed(2)}
                        <AiFillStar />
                      </div>
                      <div className="flex items-center justify-center bg-violet-200 px-2">
                        <span className="text-[12px] uppercase text-violet-900">
                          {e.type}
                        </span>
                      </div>
                    </div>
                  </Link>
                ))
              )}
            </div>
          )}
        </div>
        <div className="flex flex-col">
          <p className="flex w-full justify-between text-2xl font-semibold">
            <span>Authors</span>{' '}
          </p>
          {searchPanelContext?.searchQuery.isFetching ? (
            <Loader />
          ) : (
            <div className="flex">
              {(searchPanelContext?.searchQuery?.data?.data.authors.length ||
                0) <= 0 ? (
                <div className="flex h-full w-full items-center justify-center text-2xl font-semibold text-mv-gray">
                  No authors
                </div>
              ) : (
                searchPanelContext?.searchQuery?.data?.data.authors.map((e) => (
                  <AuthorPreview author={e} />
                ))
              )}
            </div>
          )}
        </div>
      </div>
    )
  else return null
}

const Loader = () => (
  <div className="pointer-events-none flex items-center justify-center">
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
)

export default SearchPanel
