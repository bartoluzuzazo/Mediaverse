import { Author } from '../../../models/author/Author.ts'
import { SubmitHandler, useForm } from 'react-hook-form'
import { AuthorService } from '../../../services/AuthorService.ts'
import ProfilePicker from '../form/profilePicker'
import { IoIosWarning } from 'react-icons/io'
import FormButton from '../form/button'
import { useNavigate } from '@tanstack/react-router'

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
      const response = await AuthorService.postAuthor(data)
      const id = response.data.id
      await navigate({ to: `/authors/${id}` })
    } else {
      await AuthorService.patchAuthor(data, author.id)
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
          <ProfilePicker<AuthorFormData>
            control={control}
            name={'profilePicture'}
            watch={watch}
            previousImageSrc={author?.profilePicture}
          />
          <div className="mb-2 block">
            <label>
              Name
              <input
                {...register('name', { required: 'Name is required' })}
                className="block w-full rounded-md border-2 border-slate-500 p-1"
                type="text"
              />
            </label>
            {errors.name && (
              <div className="text-red-700 flex flex-row">
                <IoIosWarning />
                <div>{errors.name.message}</div>
              </div>
            )}
          </div>

          <div className="mb-2 block">
            <label>
              Surname
              <input
                {...register('surname', { required: 'Surname is required' })}
                className="block w-full rounded-md border-2 border-slate-500 p-1"
                type="text"
              />
            </label>
            {errors.surname && (
              <div className="text-red-700 flex flex-row">
                <IoIosWarning />
                <div>{errors.surname.message}</div>
              </div>
            )}
          </div>
        </div>

        <div className="flex-1 md:ml-20">
          <div className="mb-2 block">
            <label>
              Bio
              <textarea
                {...register('bio', { required: 'Bio is required' })}
                className="block w-full rounded-md border-2 border-slate-500 p-1"
                rows={20}
              />
            </label>
            {errors.bio && (
              <div className="text-red-700 flex flex-row">
                <IoIosWarning />
                <div>{errors.bio.message}</div>
              </div>
            )}
          </div>

          <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
            {isSubmitting ? 'Submitting...' : 'Submit'}
          </FormButton>
        </div>
      </form>
    </>
  )
}

export type { AuthorFormData }
export default AuthorForm
