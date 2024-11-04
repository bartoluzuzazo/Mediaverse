import { SubmitHandler} from 'react-hook-form'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { commentService } from '../../../services/commentService.ts'
import { CommentFormData } from '../../../models/comments'
import { TextForm } from '../shared/TextForm'

type Props = {
  entryId: string
  parentQueryKeys: unknown[][]
  parentCommentId?: string
  onFormSent?: () => void
}
export const CommentForm = ({
  entryId,
  parentQueryKeys,
  parentCommentId,
  onFormSent,
}: Props) => {
  const queryClient = useQueryClient()

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
      if (onFormSent !== undefined) {
        onFormSent()
      }
    },
  })
  const onSubmit: SubmitHandler<CommentFormData> = async (data) => {
    await sendCommentMutation(data)
  }

  return (
    <TextForm
      onSubmit={onSubmit}
      name="content"
      maxLength={1000}
      defaultValues={{
        entryId: entryId,
        content: '',
        commentId: parentCommentId,
      }}
    />
  )
}

export default CommentForm
