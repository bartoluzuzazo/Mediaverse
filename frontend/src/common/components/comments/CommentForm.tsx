import { SubmitHandler, useForm } from 'react-hook-form'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { commentService } from '../../../services/commentService.ts'

interface CommentFormData {
  content: string
  entryId: string
}

type Props = {
  entryId: string
  parentQueryKey: unknown[]
}
export const CommentForm = ({ entryId, parentQueryKey }: Props) => {
  const { isAuthenticated } = useAuthContext()!
  const queryClient = useQueryClient()
  const {
    register,
    handleSubmit,
    formState: { isSubmitting },
  } = useForm<CommentFormData>({
    defaultValues: { entryId: entryId, content: '' },
  })

  const { mutateAsync: sendCommentMutation } = useMutation({
    mutationFn: async (comment: CommentFormData) => {
      return await commentService.postRootComment(comment)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: parentQueryKey,
      })
    },
  })
  const onSubmit: SubmitHandler<CommentFormData> = async (data) => {
    await sendCommentMutation(data)
  }
  if (!isAuthenticated) {
    return null
  }
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <textarea
        {...register('content', { required: 'Cannot send an empty comment' })}
        rows={10}
        className="block w-full rounded-md border-2 border-black p-1"
      />
      <button
        className={`text-white ${isSubmitting ? 'bg-violet-400' : 'bg-violet-700'}`}
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Submitting...' : 'Submit'}
      </button>
    </form>
  )
}

export default CommentForm
export type { CommentFormData }
