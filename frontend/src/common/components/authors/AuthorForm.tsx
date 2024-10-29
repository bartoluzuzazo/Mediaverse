import { Author } from '../../../models/author/Author.ts'
import { SubmitHandler, useForm } from 'react-hook-form'
import { authorService } from '../../../services/authorService.ts'
import ProfilePicker from '../form/profilePicker'
import FormButton from '../form/button'
import { useNavigate } from '@tanstack/react-router'
import FormTextArea from '../entries/FormTextArea/FormTextArea.tsx'
import FormField from '../form/FormField/FormField.tsx'

type Props = {
  author?: Author
}

type AuthorFormData = {
  id?: string
  name: string
  surname: string
  bio: string
  profilePicture: string
}
const AuthorForm = ({ author }: Props) => {
  const {
    register,
    handleSubmit,
    watch,
    control,
    formState: { errors, isSubmitting },
  } = useForm<AuthorFormData>({
    defaultValues: author
      ? {
        id: author.id,
        name: author.name,
        surname: author.surname,
        bio: author.bio,
      }
      : undefined,
  })

  const navigate = useNavigate()

  const onSubmit: SubmitHandler<AuthorFormData> = async (data) => {
    if (author == null) {
      const response = await authorService.postAuthor(data)
      const id = response.data.id
      await navigate({ to: `/authors/${id}` })
    } else {
      await authorService.patchAuthor(data, author.id)
      await navigate({ to: `/authors/${author.id}` })
    }
  }
  return (
    <>
      <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800 md:h-32"></div>

      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-col p-4 md:flex-row"
      >
        <div>
          <ProfilePicker<AuthorFormData> control={control} name={'profilePicture'} watch={watch} previousImageSrc={author?.profilePicture} />
          <FormField label={'Name'} register={register} errorValue={errors.name} registerPath={'name'} />
          <FormField label={'Surname'} register={register} errorValue={errors.surname} registerPath={'surname'} />
        </div>

        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Bio'} register={register} errorValue={errors.bio} registerPath={'bio'} />
          <div className="flex flex-row-reverse">
            <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </FormButton>
          </div>
        </div>
      </form>
    </>
  )
}

export type { AuthorFormData }
export default AuthorForm
