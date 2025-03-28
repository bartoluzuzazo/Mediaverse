import { createFileRoute, Navigate, useNavigate } from '@tanstack/react-router'
import { userService } from '../../../services/userService.ts'
import { FunctionComponent, useCallback } from 'react'
import { UserFormData } from '../../../models/user'
import { SubmitHandler, useForm } from 'react-hook-form'
import ProfilePicker from '../../../common/components/form/profilePicker'
import FormButton from '../../../common/components/form/button'
import DebounceValidatedInput from '../../../common/components/form/debounceValidatedInput'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { UpdatePasswordForm } from '../../../common/components/users/UpdatePasswordForm'

const EditUserComponent: FunctionComponent = () => {
  const { authUserData, setToken } = useAuthContext()!
  const navigate = useNavigate()
  const user = Route.useLoaderData()
  const formValues = useForm<UserFormData>({
    defaultValues: {
      id: user.id,
      email: authUserData?.email,
    },
  })
  const {
    handleSubmit,
    watch,
    control,
    formState: { isSubmitting },
  } = formValues

  const onSubmit: SubmitHandler<UserFormData> = async (data) => {
    const { token } = (await userService.patchUser(data)).data
    setToken(token)
    await navigate({ to: `/users/${user.id}` })
  }
  const validate = useCallback(
    async (value: string) => {
      if (value === authUserData?.email) {
        return null
      }
      const response = await userService.userExistsByEmail(value)
      if (response.data.exists) {
        return 'Email already taken'
      }
      return null
    },
    [authUserData?.email]
  )

  if (authUserData?.id !== user.id) {
    return <Navigate to={'/'} />
  }
  return (
    <div>
      <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800 md:h-32"></div>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-col p-4 md:flex-row"
      >
        <div>
          <ProfilePicker<UserFormData>
            control={control}
            name={'profilePicture'}
            watch={watch}
            previousImageSrc={user.profilePicture}
          />
          <UpdatePasswordForm />
        </div>

        <div className="flex-1 md:ml-20">
          <div className="mb-2 flex gap-4">
            <div className="mt-1">Email:</div>
            <div className="max-w-[400px] flex-1">
              <DebounceValidatedInput<UserFormData>
                formHookValues={formValues}
                validate={validate}
                name={'email'}
              />
            </div>
          </div>

          <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
            {isSubmitting ? 'Submitting...' : 'Submit'}
          </FormButton>
        </div>
      </form>
    </div>
  )
}

export const Route = createFileRoute('/users/edit/$id')({
  loader: async ({ params }) => {
    const response = await userService.getUser(params.id)
    return response.data
  },
  component: EditUserComponent,
})
