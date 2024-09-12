import { Comment } from '../../../models/comments'
import {
  InfiniteData,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { Page } from '../../../models/common'
import { commentService } from '../../../services/commentService.ts'
import { BiDislike, BiLike, BiSolidDislike, BiSolidLike } from 'react-icons/bi'

type Props = {
  comment: Comment
  parentPage: number
  parentQueryKey: unknown[]
}
export const CommentVotes = ({
  comment,
  parentPage,
  parentQueryKey,
}: Props) => {
  const parentIndex = parentPage - 1
  const queryClient = useQueryClient()

  const { mutate: sendVoteMutation } = useMutation({
    mutationFn: async (newVote?: boolean) => {
      if (newVote == undefined) {
        await commentService.deleteVote(comment.id)
      } else if (comment.usersVote == null) {
        await commentService.postVote({
          commentId: comment.id,
          isPositive: newVote,
        })
      } else {
        await commentService.putVote({
          commentId: comment.id,
          isPositive: newVote,
        })
      }
    },
    onMutate: (newVote) => {
      const downvoteChange =
        comment.usersVote === false ? -1 : newVote === false ? 1 : 0
      const upvoteChange =
        comment.usersVote === true ? -1 : newVote === true ? 1 : 0
      const previousData =
        queryClient.getQueryData<InfiniteData<Page<Comment>>>(parentQueryKey)
      queryClient.setQueryData(
        parentQueryKey,
        (data: InfiniteData<Page<Comment>>) => {
          const copy = { ...data }
          copy.pages = [...data.pages]
          copy.pages[parentIndex] = { ...copy.pages[parentIndex] }
          copy.pages[parentIndex].contents = copy.pages[
            parentIndex
          ].contents.map((c) => {
            if (c.id != comment.id) {
              return c
            } else {
              return {
                ...c,
                usersVote: newVote,
                downvotes: c.downvotes + downvoteChange,
                upvotes: c.upvotes + upvoteChange,
              }
            }
          })
          return copy
        }
      )
      return { previousData }
    },
    onError: (_err, _newVote, context) => {
      queryClient.setQueryData(parentQueryKey, context!.previousData)
    },
  })

  const { isAuthenticated } = useAuthContext()!
  const sendIfAuthorized = (newVote?: boolean) => {
    if (isAuthenticated) {
      sendVoteMutation(newVote)
    }
  }
  return (
    <div className="flex items-end gap-0.5 text-xl text-slate-700">
      <span>{comment.upvotes}</span>
      {comment.usersVote ? (
        <BiSolidLike
          className={`mr-3 text-2xl ${isAuthenticated ? 'hover:scale-125' : ''}`}
          onClick={() => {
            sendIfAuthorized(undefined)
          }}
        />
      ) : (
        <BiLike
          className={`mr-3 text-2xl ${isAuthenticated ? 'hover:scale-125' : ''}`}
          onClick={() => sendIfAuthorized(true)}
        />
      )}
      <span>{comment.downvotes}</span>
      {!comment.usersVote && comment.usersVote != undefined ? (
        <BiSolidDislike
          className={`text-2xl ${isAuthenticated ? 'hover:scale-125' : ''}`}
          onClick={() => sendIfAuthorized(undefined)}
        />
      ) : (
        <BiDislike
          className={`text-2xl ${isAuthenticated ? 'hover:scale-125' : ''}`}
          onClick={() => sendIfAuthorized(false)}
        />
      )}
    </div>
  )
}
