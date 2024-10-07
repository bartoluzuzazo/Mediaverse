import { Comment, GetCommentsParams } from '../../../models/comments'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { useInfiniteQuery } from '@tanstack/react-query'
import { commentService } from '../../../services/commentService.ts'
import { CommentView } from './CommentView.tsx'
import { SetStateAction, useState } from 'react'
import CommentForm from './CommentForm.tsx'
import { useToggle } from 'usehooks-ts'
import { FaSquarePlus } from 'react-icons/fa6'

type Props = {
  parentComment: Comment
  commentParams: Omit<GetCommentsParams, 'page'>
  parentQueryKey: unknown[]
  isReplying: boolean
  setIsReplying: React.Dispatch<SetStateAction<boolean>>
}

export const Subcomments = ({
  parentComment,
  commentParams,
  isReplying,
  parentQueryKey,
  setIsReplying,
}: Props) => {
  const { isAuthenticated } = useAuthContext()!
  const queryKey = ['GET_SUBCOMMENTS', parentComment.id, { commentParams }]
  const [isNotFirstRequest, setIsNotFirstRequest] = useState(false)
  const [isCollapsed, toggleCollapsed] = useToggle(false)
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
    enabled: isNotFirstRequest,
    getNextPageParam: (lastPage) =>
      lastPage.hasNext ? lastPage.currentPage + 1 : null,
  })
  if (isCollapsed) {
    return (
      <div
        onClick={toggleCollapsed}
        className="flex items-center gap-3 text-xl text-slate-700 cursor-pointer"
      >
        <FaSquarePlus /> Show comments
      </div>
    )
  }

  const onFormSent = () => {
    setIsReplying(false)
    if (!isNotFirstRequest) {
      setIsNotFirstRequest(true)
      fetchNextPage()
    }
  }
  return (
    <>
      {isReplying && (
        <CommentForm
          entryId={parentComment.entryId}
          parentQueryKeys={[parentQueryKey, queryKey]}
          parentCommentId={parentComment.id}
          onFormSent={onFormSent}
        />
      )}
      {parentComment.subcommentCount > 0 && (
        <>
          <div className="flex">
            <div
              onClick={toggleCollapsed}
              className="w-4 border-l-[4px] border-slate-400 hover:border-slate-700 md:w-6 cursor-pointer"
            ></div>
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
          {(data?.pages[data?.pages.length - 1].hasNext ||
            data === undefined) && (
            <button
              className="mb-3 rounded-3xl bg-slate-200 px-4 py-2 font-semibold text-slate-800"
              onClick={() => {
                setIsNotFirstRequest(true)
                fetchNextPage()
              }}
            >
              Load {data && 'more'} comments
            </button>
          )}
        </>
      )}
    </>
  )
}
