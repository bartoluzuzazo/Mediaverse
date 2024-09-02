import { FunctionComponent } from 'react'
import { AuthFormProps } from './authForm'
import { Register } from '../../../models/authentication/register'
import { useForm } from 'react-hook-form'
import { useMutation } from '@tanstack/react-query'
import { AuthService } from '../../../services/authService'
import FormButton from '../form/button'
import FormInput from '../form/input'

const RegisterForm: FunctionComponent<AuthFormProps> = ({
  setLoginPanelVisible,
}) => {
  const { register, handleSubmit, reset } = useForm<Register>()

  const registerMutation = useMutation({
    mutationFn: async (data: Register) => await AuthService.register(data),
    onSuccess() {
      reset()
    },
  })

  const onSubmit = (formData: Register) => {
    registerMutation.mutate(formData)
  }

  return (
    <form className="flex flex-col gap-2" onSubmit={handleSubmit(onSubmit)}>
      <div>
        <h2 className="text-xl font-semibold">Register</h2>
      </div>
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'username',
          ...register('username', { required: 'username is required' }),
        }}
      />
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'email',
          ...register('email', { required: 'email is required' }),
        }}
      />
      <FormInput
        inputProps={{
          type: 'password',
          placeholder: 'password',
          ...register('password', { required: 'password is required' }),
        }}
      />

      <FormButton
        buttonType="purple"
        buttonProps={{
          type: 'submit',
          disabled: registerMutation.isPending,
        }}
      >
        {registerMutation.isPending ? 'registering...' : 'register'}
      </FormButton>
      <p className="text-xs text-mv-red">
        {registerMutation.data?.data?.exception?.message || ''}
      </p>
      <p className="text-xs">
        Already have an account?{' '}
        <span
          className="text-mv-purple hover:cursor-pointer hover:underline"
          onClick={() => setLoginPanelVisible(true)}
        >
          Log in
        </span>
      </p>
    </form>
  )
}

export default RegisterForm
