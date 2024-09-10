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
      <div className="mb-4 rounded-xl p-3 shadow-md">
        <div className="flex gap-6">
          <div>
            <img
              src={imgSrc}
              alt="users profile"
              className="picture aspect-square w-24 rounded-full border-2 object-cover shadow-md"
            />
            <div className="text-center">{comment.username}</div>
          </div>
          <div className="flex-1">{comment.content}</div>
        </div>
        <div className="flex justify-end">
          {isAuthenticated && (
            <button
              className={`${isReplying ? 'bg-violet-700' : 'bg-violet-500'} mr-auto rounded-md text-white`}
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
