import { createFileRoute } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import FormButton from '../../../common/components/form/button'
import { IoIosWarning } from 'react-icons/io'
import ProfilePicker from '../../../common/components/form/profilePicker'
import { CreateAuthorCommand } from '../../../models/author/CreateAuthorCommand.ts'
import { AuthorService } from '../../../services/AuthorService.ts'

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

    const onSubmit: SubmitHandler<CreateAuthorCommand> = async (data) => {
      await AuthorService.postAuthor(data)
      console.log(data)
    }
    return (
      <>
        <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800"></div>

        <form
          onSubmit={handleSubmit(onSubmit)}
          className="flex flex-col p-4 md:flex-row"
        >
          <div>
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
          </div>

          <div className="flex-1 md:ml-20">
            <div className="mb-2 block">
              <label>
                Bio
                <textarea
                  {...register('bio', { required: 'Bio is required' })}
                  className="block w-full rounded-sm border-2 border-slate-500"
                  rows={20}
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
          </div>
        </form>
      </>
    )
  },
})
