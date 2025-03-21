import { useMutation } from '@tanstack/react-query'
import { FunctionComponent } from 'react'
import { useForm } from 'react-hook-form'
import { useAuthContext } from '../../../context/auth/useAuthContext'
import { Login } from '../../../models/authentication/login'
import { AuthService } from '../../../services/authService'
import FormButton from '../form/button'
import FormInput from '../form/input'
import { AuthFormProps } from './authForm'

const LoginForm: FunctionComponent<AuthFormProps> = ({
  setLoginPanelVisible,
}) => {
  const { register, handleSubmit } = useForm<Login>()

  const authContext = useAuthContext()

  const loginMutation = useMutation({
    mutationFn: async (data: Login) => await AuthService.login(data),
    onSuccess({ data }) {
      authContext?.setToken(data.token)
    },
  })

  const onSubmit = (formData: Login) => {
    loginMutation.mutate(formData)
  }

  return (
    <form className="flex flex-col gap-2" onSubmit={handleSubmit(onSubmit)}>
      <div>
        <h2 className="text-xl font-semibold">Login</h2>
      </div>
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
          disabled: loginMutation.isPending,
        }}
      >
        {loginMutation.isPending ? 'logging in...' : 'login'}
      </FormButton>
      <p className="text-xs text-mv-red">
        {loginMutation.isError && 'something went wrong'}
      </p>
      <p className="text-xs">
        Don't have an account?{' '}
        <span
          className="text-mv-purple hover:cursor-pointer hover:underline"
          onClick={() => setLoginPanelVisible(false)}
        >
          Sign up
        </span>
      </p>
    </form>
  )
}

export default LoginForm
