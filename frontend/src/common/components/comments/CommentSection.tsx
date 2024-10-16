import { useInfiniteQuery } from '@tanstack/react-query'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { commentService } from '../../../services/commentService.ts'
import { CommentOrder, GetCommentsParams } from '../../../models/comments'
import { OrderDirection } from '../../../models/common'
import { Dispatch, FunctionComponent, SetStateAction, useEffect, useRef, useState } from 'react'
import { CommentView } from './CommentView.tsx'
import { useInView } from 'framer-motion'
import CommentForm from './CommentForm.tsx'
import { DropdownButton, DropDownChoice } from '../shared/DropdownChoice'
import { FaArrowDown, FaArrowUp, FaClock, FaMedal } from 'react-icons/fa'
import { RiNumbersFill } from 'react-icons/ri'
import { Oval } from 'react-loader-spinner'


type CommentSortProps = {
  setCommentParams: Dispatch<SetStateAction<Omit<GetCommentsParams, 'page'>>>
}
const CommentSort: FunctionComponent<CommentSortProps> = ({ setCommentParams }) => {

  const onSortDropdownChange = (order: CommentOrder) => {
    setCommentParams(params => {
      return { ...params, order }
    })
  }

  const SortDropdownOptions = [
    { element: <DropdownButton icon={<RiNumbersFill />} text='vote count' />, value: CommentOrder.voteCount },
    { element: <DropdownButton icon={<FaMedal />} text='votes' />, value: CommentOrder.votes },
    { element: <DropdownButton icon={<FaClock />} text='time' />, value: CommentOrder.createdAt },
  ]
  const onDirectionDropdownChange = (direction: OrderDirection) => {
    setCommentParams(params => {
      return { ...params, direction }
    })
  }

  const directionDropdownOptions = [
    { element: <DropdownButton icon={<FaArrowDown />} text='descending' />, value: OrderDirection.Descending },
    { element: <DropdownButton icon={<FaArrowUp />} text='ascending' />, value: OrderDirection.Ascending },
  ]
  return <div className='flex items-center gap-x-12 gap-y-3 flex-wrap'>
    <div className='flex items-center'>
      <span className='text-lg font-semibold text-slate-700 mr-3'>Sort by:</span>
      <DropDownChoice options={SortDropdownOptions} onChange={onSortDropdownChange} />
    </div>
    <div className='flex items-center'>
      <span className='text-lg font-semibold text-slate-700 mr-3'>Direction</span>
      <DropDownChoice options={directionDropdownOptions} onChange={onDirectionDropdownChange} />
    </div>
  </div>
}

type CommentSectionProps = { entryId: string }
const CommentSection = ({ entryId }: CommentSectionProps) => {
  const { isAuthenticated } = useAuthContext()!
  const [commentParams, setCommentParams] = useState<Omit<GetCommentsParams, 'page'>>({
    order: CommentOrder.votes,
    direction: OrderDirection.Descending,
    size: 20,
  })
  const queryKey = ['GET_ENTRY_COMMENTS', entryId, { commentParams }, { isAuthenticated }]
  const { data, fetchNextPage, isFetching } = useInfiniteQuery({
    queryKey: queryKey,
    queryFn: async ({ pageParam }) => {
      if (isAuthenticated) {
        return await commentService
          .getRootCommentsAuthorized(entryId, {
            page: pageParam,
            ...commentParams,
          })
          .then((res) => res.data)
      } else {
        return await commentService
          .getRootCommentsUnauthorized(entryId, {
            page: pageParam,
            ...commentParams,
          })
          .then((res) => res.data)
      }
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) =>
      lastPage.hasNext ? lastPage.currentPage + 1 : null,
  })

  const viewBoxRef = useRef(null)
  const isInView = useInView(viewBoxRef)

  useEffect(() => {
    if (isInView) {
      fetchNextPage()
    }
  }, [fetchNextPage, isInView])


  return (
    <div>
      <CommentForm entryId={entryId} parentQueryKeys={[queryKey]} />
      <CommentSort setCommentParams={setCommentParams} />
      {data &&
        data.pages.map((page) =>
          page.contents.map((c) => {
            return (
              <CommentView
                comment={c}
                key={c.id}
                parentPage={page.currentPage}
                parentQueryKey={queryKey}
                commentParams={commentParams}
              />
            )
          }),
        )}
      {isFetching &&
        <div className='pointer-events-none flex items-center justify-center'>
          <Oval
            visible={true}
            height='80'
            width='80'
            color='#5b21b6'
            secondaryColor=''
            ariaLabel='oval-loading'
            wrapperStyle={{}}
            wrapperClass=''
          />
        </div>
      }
      <div ref={viewBoxRef} className='min-h-1' />

    </div>
  )
}

export default CommentSection
