import { FunctionComponent } from 'react'
import { ModalButton } from '../../shared/ModalButton'
import { SubmitHandler, useForm } from 'react-hook-form'
import { useNavigate } from '@tanstack/react-router'
import { AmaSessionFormData } from '../../../../models/amaSessions'
import FormField from '../../form/FormField/FormField.tsx'
import FormTextArea from '../../entries/FormTextArea/FormTextArea.tsx'
import FormButton from '../../form/button'
import { FaQuestion } from 'react-icons/fa'
import { amaSessionService } from '../../../../services/amaSessionService.ts'

interface AmaSessionFormProps {
  authorId: string
}

export const AmaSessionForm: FunctionComponent<AmaSessionFormProps> = ({
  authorId,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<AmaSessionFormData>({
    defaultValues: {
      authorId,
    },
  })
  const navigate = useNavigate()
  const onSubmit: SubmitHandler<AmaSessionFormData> = async (data) => {
    const response = await amaSessionService.postAmaSession(data)
    const id = response.data.id

    await navigate({to: '/ama-sessions/$id', params: {id}})
  }
  return (
    <form onSubmit={handleSubmit(onSubmit)} className="p-4">
      <FormField
        label={'Title'}
        register={register}
        registerPath={'title'}
        errorValue={errors.title}
      />
      <FormTextArea
        rows={10}
        label={'Description'}
        registerPath={'description'}
        register={register}
        errorValue={errors.description}
      />
      <FormField
        type='datetime-local'
        label={'Start time'}
        register={register}
        registerPath={'start'}
        errorValue={errors.start}
      />
      <FormField
        type='datetime-local'
        label={'End time'}
        register={register}
        registerPath={'end'}
        errorValue={errors.end}
      />
      <div className="flex flex-row-reverse">
        <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
          {isSubmitting ? 'Submitting...' : 'Submit'}
        </FormButton>
      </div>
    </form>
  )
}

interface CreateAmaComponentProps {
  authorId: string
}

export const CreateAmaComponent: FunctionComponent<CreateAmaComponentProps> = ({
  authorId,
}) => {
  return (
    <ModalButton icon={<FaQuestion/>} text="Create Ama">
      <AmaSessionForm authorId={authorId} />
    </ModalButton>
  )
}