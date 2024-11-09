import {
  AmaQuestion,
  AmaQuestionAnswerFormData,
} from '../../../../models/amaSessions'
import { FunctionComponent } from 'react'
import { TextForm } from '../../shared/TextForm'
import { SubmitHandler } from 'react-hook-form'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { amaSessionService } from '../../../../services/amaSessionService.ts'

type Props = {
  question: AmaQuestion
  parentQueryKey: unknown[]
  toggleIsReplying: () => void
}

export const AmaQuestionReplyForm: FunctionComponent<Props> = ({
  question,
  parentQueryKey,
  toggleIsReplying,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync: sendAnswerAsync } = useMutation({
    mutationFn: async (data: AmaQuestionAnswerFormData) =>
      await amaSessionService.putAmaSessionAnswer(question.id, data),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: parentQueryKey })
      toggleIsReplying()
    },
  })
  const onSubmit: SubmitHandler<AmaQuestionAnswerFormData> = async (data) => {
    await sendAnswerAsync(data)
  }
  return (
    <TextForm<AmaQuestionAnswerFormData>
      defaultValues={{ answer: '' }}
      maxLength={300}
      name="answer"
      onSubmit={onSubmit}
    />
  )
}
