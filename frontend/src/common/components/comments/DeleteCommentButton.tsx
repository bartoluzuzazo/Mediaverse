import { FunctionComponent } from 'react'
import { Comment } from '../../../models/comments'
import { FaTrash } from 'react-icons/fa'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { commentService } from '../../../services/commentService.ts'

type Props = {
  comment: Comment,
  invalidationKey: unknown[]
}

export const DeleteCommentButton: FunctionComponent<Props> = ({ comment, invalidationKey }) => {
  const queryClient = useQueryClient()
  const { mutateAsync: deleteCommentAsync } = useMutation({
    mutationFn: async () => {
      await commentService.deleteComment(comment.id)
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: invalidationKey,
      })
    },
  })

  return (
    <button className='bg-red-700 text-white px-2.5 py-1.5 mr-3 md:mr-6' onClick={async () => {
      await deleteCommentAsync()
    }}>
      <FaTrash  />
    </button>
  )
}