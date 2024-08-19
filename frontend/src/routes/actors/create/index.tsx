import { createFileRoute } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import FormButton from '../../../common/components/form/button'
import { IoIosWarning } from 'react-icons/io'
import ProfilePicker from '../../../common/components/form/profilePicker'

interface CreateAuthorCommand {
  name: string
  surname: string
  bio: string
  profilePicture: File
}

export const Route = createFileRoute('/actors/create/')({
  component: () => {
    const {
      register,
      handleSubmit,
      watch,
      control,
      formState: { errors },
    } = useForm<CreateAuthorCommand>()
    const file = watch('profilePicture')

    console.log(file)
    const onSubmit: SubmitHandler<CreateAuthorCommand> = (data) => {
      console.log(data)
    }
    return (
      <>
        <div className="h-20 bg-violet-800"></div>

        <form onSubmit={handleSubmit(onSubmit)} className="p-4">
          <ProfilePicker<CreateAuthorCommand>
            control={control}
            name={'profilePicture'}
            file={file}
          />
          <div className="mb-2 block">
            <label>
              Name
              <input
                {...register('name', { required: 'Name is required' })}
                className="block h-8 w-full rounded-sm border-2 border-slate-500"
                type="text"
              />
            </label>
            {errors.name && (
              <div className="text-red-700">
                <IoIosWarning /> {errors.name.message}
              </div>
            )}
          </div>

          <div className="mb-2 block">
            <label>
              Surname
              <input
                {...register('surname', { required: 'Surname is required' })}
                className="block h-8 w-full rounded-sm border-2 border-slate-500"
                type="text"
              />
            </label>
            {errors.surname && (
              <div className="text-red-700">
                <IoIosWarning />
                {errors.surname.message}
              </div>
            )}
          </div>

          <div className="mb-2 block">
            <label>
              Bio
              <textarea
                {...register('bio', { required: 'Bio is required' })}
                className="block w-full rounded-sm border-2 border-slate-500"
              />
            </label>
            {errors.bio && (
              <div className="text-red-700">
                <IoIosWarning className="inline" />
                {errors.bio.message}
              </div>
            )}
          </div>

          <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
            Submit
          </FormButton>
        </form>
      </>
    )
  },
})
