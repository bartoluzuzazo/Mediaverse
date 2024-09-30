import { SubmitHandler, useForm } from 'react-hook-form'
import { UpdatePasswordFormData } from '../../../../models/user'
import { FunctionComponent } from 'react'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { useToggle } from 'usehooks-ts'
import { userService } from '../../../../services/userService.ts'
import { AnimatePresence, motion } from 'framer-motion'
import FormField from '../../form/FormField/FormField.tsx'
import { useMutation } from '@tanstack/react-query'
import { IoIosWarning } from 'react-icons/io'
import { isAxiosError } from 'axios'

export const UpdatePasswordForm: FunctionComponent = () => {
  const [isToggled, toggle] = useToggle()
  const data = useAuthContext()!
  const { authUserData } = data
  const { handleSubmit, register, formState: { errors }, watch, reset } = useForm<UpdatePasswordFormData>()
  const newPassword = watch('newPassword')

  const updatePasswordMutation = useMutation({
    mutationFn: userService.putPassword,
    onSuccess: () => {
      reset()
      toggle()
    },
  })


  const onSubmit: SubmitHandler<UpdatePasswordFormData> = async (data) => {
    updatePasswordMutation.mutate(data)
  }
  if (!authUserData) {
    return <></>
  }
  const error = updatePasswordMutation.error
  const errorMessage = !error ? null :
    isAxiosError(error) && error.response?.status === 401 ? 'Bad password' : 'Something went wrong'

  return (
    <div className='overflow-hidden'>
      <button onClick={toggle}
              className='mb-3 rounded-3xl bg-slate-200 px-4 py-2 font-semibold text-slate-800'
              type='button'
      >
        Update password
      </button>
      <AnimatePresence>
        {isToggled &&
          <motion.div initial={{ x: -300, opacity: 0.3 }} animate={{ x: 0, opacity: 1 }}
                      exit={{ x: -300, opacity: 0.3 }}>

            <div>
              <FormField<UpdatePasswordFormData> label={'Old password'} name={'oldPassword'} register={register}
                                                 errorValue={errors.oldPassword} type="password"/>
              <FormField<UpdatePasswordFormData> label={'New password'} name={'newPassword'} register={register}
                                                 errorValue={errors.newPassword} type="password"/>
              <FormField<UpdatePasswordFormData> label={'Repeat password'} name={'repeatPassword'}
                                                 register={register} type="password"
                                                 errorValue={errors.repeatPassword}
                                                 rules={{ validate: repeatedPassword => repeatedPassword == newPassword || 'Passwords do not match'}} />
              <button onClick={handleSubmit(onSubmit)}>Update</button>
              {errorMessage &&
                <div className='text-red-700 flex flex-row items-center'>
                  <IoIosWarning />
                  <div>{errorMessage}</div>
                </div>
              }
            </div>
          </motion.div>
        }
      </AnimatePresence>
    </div>
  )
}