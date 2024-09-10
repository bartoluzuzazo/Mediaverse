import { Comment, GetCommentsParams } from '../../../models/comments'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { useInfiniteQuery, useQueryClient } from '@tanstack/react-query'
import { commentService } from '../../../services/commentService.ts'
import { CommentView } from './CommentView.tsx'
import { useEffect } from 'react'

type Props = {
  parentComment: Comment
  commentParams: Omit<GetCommentsParams, 'page'>
}

export const Subcomments = ({ parentComment, commentParams }: Props) => {
  const { isAuthenticated } = useAuthContext()!
  const queryClient = useQueryClient()
  const queryKey = ['GET_SUBCOMMENTS', parentComment.id, { commentParams }]
  const { data, fetchNextPage } = useInfiniteQuery({
    queryKey: queryKey,
    queryFn: async ({ pageParam }) => {
      if (isAuthenticated) {
        return await commentService
          .getSubcommentsAuthorized(parentComment.id, {
            ...commentParams,
            page: pageParam,
          })
          .then((res) => {
            console.log(res.data)
            return res.data
          })
      } else {
        return await commentService
          .getSubcommentsUnauthorized(parentComment.id, {
            ...commentParams,
            page: pageParam,
          })
          .then((res) => res.data)
      }
    },
    initialPageParam: 1,
    enabled: false,
    getNextPageParam: (lastPage) =>
      lastPage.hasNext ? lastPage.currentPage + 1 : null,
  })
  console.log(data?.pages[0].contents[0].id)
  return (
    <>
      <div className="flex">
        <div className="w-6 border-l-2 border-slate-400 hover:border-slate-700"></div>
        <div className="flex-1">
          {data &&
            data.pages.map((page) => {
              return page.contents.map((c) => {
                return (
                  <CommentView
                    comment={c}
                    parentPage={page.currentPage}
                    parentQueryKey={queryKey}
                    commentParams={commentParams}
                    key={c.id}
                  />
                )
              })
            })}
        </div>
      </div>
      {(data?.pages[data?.pages.length - 1].hasNext || data === undefined) && (
        <button
          className="bg-violet-700 text-white"
          onClick={() => {
            fetchNextPage()
          }}
        >
          Load {data && 'more'} comments
        </button>
      )}
    </>
  )
}
