import { FunctionComponent } from 'react'
import { TextForm } from '../../shared/TextForm'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { AmaQuestionFormData } from '../../../../models/amaSessions'
import { amaSessionService } from '../../../../services/amaSessionService.ts'
import { SubmitHandler } from 'react-hook-form'

type Props = {
  amaSessionId: string
  parentQueryKeys: unknown[]
}

export const AmaQuestionForm: FunctionComponent<Props> = ({
  amaSessionId,
  parentQueryKeys,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync: sendQuestionMutation } = useMutation({
    mutationFn: async (data: AmaQuestionFormData) => {
      return await amaSessionService.postAmaQuestion(amaSessionId, data)
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: parentQueryKeys })
    },
  })
  const onSubmit: SubmitHandler<AmaQuestionFormData> = async (data) => {
    return await sendQuestionMutation(data)
  }
  return (
    <TextForm<AmaQuestionFormData>
      onSubmit={onSubmit}
      name="content"
      defaultValues={{ content: '' }}
      maxLength={1000}
    />
  )
}
