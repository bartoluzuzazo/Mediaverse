import { Comment, GetCommentsParams } from '../../../models/comments'
import { CommentVotes } from './CommentVotes.tsx'
import { Subcomments } from './Subcomments.tsx'
import { useState } from 'react'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'

type Props = {
  comment: Comment
  parentPage: number
  parentQueryKey: unknown[]
  commentParams: Omit<GetCommentsParams, 'page'>
}
export const CommentView = ({
  comment,
  parentPage,
  parentQueryKey,
  commentParams,
}: Props) => {
  const imgSrc = 'data:image/*;base64,' + comment.userProfile
  const [isReplying, setIsReplying] = useState(false)
  const { isAuthenticated } = useAuthContext()!
  return (
    <>
      <div className="mb-4 mt-3 rounded-xl p-3 shadow-md">
        <div className="flex gap-6">
          <div>
            <img
              src={imgSrc}
              alt="users profile"
              className="picture aspect-square w-16 rounded-full border-2 object-cover shadow-md md:w-24"
            />
            <div className="mb-3 text-center font-semibold">
              {comment.username}
            </div>
          </div>
          <div className="flex-1">{comment.content}</div>
        </div>
        <div className="flex items-baseline justify-end">
          {isAuthenticated && (
            <button
              className={`${isReplying ? 'bg-slate-900' : 'bg-slate-700'} mr-auto rounded-md px-3 py-1.5 text-white md:px-4 md:py-2`}
              onClick={() => setIsReplying((r) => !r)}
            >
              Reply
            </button>
          )}
          <CommentVotes
            comment={comment}
            parentPage={parentPage}
            parentQueryKey={parentQueryKey}
          />
        </div>
      </div>

      <Subcomments
        parentComment={comment}
        commentParams={commentParams}
        parentQueryKey={parentQueryKey}
        isReplying={isReplying}
        setIsReplying={setIsReplying}
      />
    </>
  )
}
