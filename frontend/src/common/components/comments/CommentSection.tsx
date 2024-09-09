import { useInfiniteQuery } from '@tanstack/react-query'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { commentService } from '../../../services/commentService.ts'
import { CommentOrder, GetCommentsParams } from '../../../models/comments'
import { OrderDirection } from '../../../models/common'
import { useEffect, useRef, useState } from 'react'
import { CommentView } from './CommentView.tsx'
import { useInView } from 'framer-motion'
import CommentForm from './CommentForm.tsx'
import { useQueryClient } from '@tanstack/react-query'

type Props = { entryId: string }
const CommentSection = ({ entryId }: Props) => {
  const { isAuthenticated } = useAuthContext()!
  const [commentParams] = useState<Omit<GetCommentsParams, 'page'>>({
    order: CommentOrder.votes,
    direction: OrderDirection.Descending,
    size: 2,
  })
  const { data, fetchNextPage, isFetchingNextPage } = useInfiniteQuery({
    queryKey: ['GET_ENTRY_COMMENTS', entryId, { commentParams }],
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
          .getRootCommentsAuthorized(entryId, {
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

  const queryClient = useQueryClient()
  const invalidateEntryComments = () => {
    queryClient.invalidateQueries({
      queryKey: ['GET_ENTRY_COMMENTS', entryId, { commentParams }],
    })
  }

  useEffect(() => {
    if (isInView) {
      fetchNextPage()
    }
  }, [fetchNextPage, isInView])

  const comments = data?.pages.flatMap((page) => page.contents)
  return (
    <div>
      <CommentForm
        entryId={entryId}
        invalidateParentComments={invalidateEntryComments}
      />
      {comments &&
        comments.map((c) => {
          return <CommentView comment={c} key={c.id} />
        })}
      <div ref={viewBoxRef} className="min-h-1">
        {isFetchingNextPage && 'Loading...'}
      </div>
    </div>
  )
}

export default CommentSection
