import { FunctionComponent } from 'react'
import { Review, ReviewFormData } from '../../../../models/review'
import { SubmitHandler, useForm } from 'react-hook-form'
import { SubmitButton } from '../../form/submitButton'
import { RatingField } from '../../form/ratingField'
import FormField from '../../form/FormField/FormField.tsx'
import { MarkdownField } from '../../form/markdownField'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { reviewService } from '../../../../services/reviewService.ts'
import { useNavigate } from '@tanstack/react-router'

type Props = {
  review?: Review
  entryId: string
}

export const ReviewForm: FunctionComponent<Props> = ({ review, entryId }) => {
  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ReviewFormData>({
    defaultValues: review
      ? {
          title: review.title,
          content: review.content,
          grade: review.grade,
        }
      : undefined,
  })
  const { authUserData } = useAuthContext()!
  const navigate = useNavigate()
  const onSubmit: SubmitHandler<ReviewFormData> = async (data) => {
    await reviewService.putReview(entryId, data)
    await navigate({
      to: '/reviews/$userId/entries/$entryId',
      params: { entryId, userId: authUserData!.id },
    })
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <FormField<ReviewFormData>
        registerPath="title"
        label="Title"
        register={register}
        errorValue={errors.title}
      />
      <RatingField control={control} name="grade" max={5} />
      <MarkdownField control={control} name="content" />
      <SubmitButton isSubmitting={isSubmitting} />
    </form>
  )
}
