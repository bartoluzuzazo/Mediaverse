import { SubmitHandler, useForm } from 'react-hook-form'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { commentService } from '../../../services/commentService.ts'
import { SetStateAction } from 'react'
import { CommentFormData } from '../../../models/comments'

type Props = {
  entryId: string
  parentQueryKeys: unknown[][]
  parentCommentId?: string
  setIsReplying?: React.Dispatch<SetStateAction<boolean>>
}
export const CommentForm = ({
  entryId,
  parentQueryKeys,
  parentCommentId,
  setIsReplying,
}: Props) => {
  const { isAuthenticated } = useAuthContext()!
  const queryClient = useQueryClient()
  const {
    register,
    handleSubmit,
    formState: { isSubmitting },
    reset,
  } = useForm<CommentFormData>({
    defaultValues: {
      entryId: entryId,
      content: '',
      commentId: parentCommentId,
    },
  })

  const { mutateAsync: sendCommentMutation } = useMutation({
    mutationFn: async (comment: CommentFormData) => {
      if (comment.commentId == undefined) {
        return await commentService.postRootComment(comment)
      } else {
        return await commentService.postSubcomment(comment)
      }
    },
    onSuccess: () => {
      for (const key of parentQueryKeys) {
        queryClient.invalidateQueries({
          queryKey: key,
        })
      }
      reset()
      if (setIsReplying !== undefined) {
        setIsReplying(false)
      }
    },
  })
  const onSubmit: SubmitHandler<CommentFormData> = async (data) => {
    await sendCommentMutation(data)
  }
  if (!isAuthenticated) {
    return null
  }
  return (
    <form onSubmit={handleSubmit(onSubmit)} className="mb-6">
      <textarea
        {...register('content', { required: 'Cannot send an empty comment' })}
        rows={10}
        className="block w-full rounded-md border-2 border-black p-1"
      />
      <button
        className={`mt-2 text-white ${isSubmitting ? 'bg-violet-400' : 'bg-violet-700'}`}
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Submitting...' : 'Submit'}
      </button>
    </form>
  )
}

export default CommentForm
