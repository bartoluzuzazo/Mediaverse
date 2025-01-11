import { Comment, GetCommentsParams } from '../../../models/comments'
import { CommentVotes } from './CommentVotes.tsx'
import { Subcomments } from './Subcomments.tsx'
import { useState } from 'react'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import defaultImgUrl from '/person-icon.png'
import { AuthorizedView } from '../auth/AuthorizedView'
import { DeleteCommentButton } from './DeleteCommentButton.tsx'
import { Link } from '@tanstack/react-router'

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
  const imgSrc = comment.userProfile ? 'data:image/*;base64,' + comment.userProfile : defaultImgUrl
  const [isReplying, setIsReplying] = useState(false)
  const { isAuthenticated } = useAuthContext()!
  return (
    <>
      <div className={`mb-4 mt-3 rounded-xl p-3 shadow-md ${comment.isDeleted ? 'opacity-75' : ''}`}>
        <div className="flex gap-6">
          <Link to={comment.userId ? `/users/${comment.userId}` : ""} className="text-black">
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
          </Link>
          <div className="flex-1">{comment.content}</div>
        </div>
        <div className="flex items-baseline justify-end">
          {isAuthenticated && !comment.isDeleted && (
            <>
              <button
                className={`${isReplying ? 'bg-slate-900' : 'bg-slate-700'} mr-auto rounded-md px-3 py-1.5 text-white md:px-4 md:py-2`}
                onClick={() => setIsReplying((r) => !r)}
              >
                Reply
              </button>
              <AuthorizedView requiredUserId={comment.userId}>
                <DeleteCommentButton comment={comment} invalidationKey={parentQueryKey} />
              </AuthorizedView>
            </>
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
