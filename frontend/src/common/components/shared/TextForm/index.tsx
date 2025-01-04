import {
  DefaultValues,
  FieldValues,
  Path,
  SubmitHandler,
  useForm,
} from 'react-hook-form'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import { useEffect, useState } from 'react'
import { IoIosWarning } from 'react-icons/io'

export const TextForm = <T extends FieldValues>({
  onSubmit,
  name,
  defaultValues,
  maxLength,
}: {
  onSubmit: SubmitHandler<T>
  name: Path<T>
  defaultValues: DefaultValues<T>
  maxLength: number
}) => {
  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors },
    reset,
  } = useForm<T>({
    defaultValues,
  })

  const [isSafeToReset, setIsSafeToReset] = useState(false)
  const submitAndReset: SubmitHandler<T> = (data, event) => {
    setIsSafeToReset(true)
    return onSubmit(data, event)
  }
  useEffect(() => {
    if (!isSafeToReset) {
      return
    }
    reset()
    setIsSafeToReset(false)
  }, [isSafeToReset, setIsSafeToReset, reset])

  const { isAuthenticated } = useAuthContext()!
  if (!isAuthenticated) {
    return null
  }

  return (
    <form onSubmit={handleSubmit(submitAndReset)} className="mb-6">
      <textarea
        {...register(name, {
          required: `${name} is required`,
          maxLength: {
            value: maxLength,
            message: `Maximum length is ${maxLength}`,
          },
        })}
        rows={10}
        className="block w-full rounded-md border-2 border-black p-1"
      />
      {errors[name] && (
        <div className="flex flex-row text-red-700">
          <IoIosWarning />
          <div>Length must be between 1 and {maxLength}</div>
        </div>
      )}
      <button
        className={`mt-2 text-white ${isSubmitting ? 'bg-violet-400' : 'bg-violet-700'}`}
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Submitting...' : 'Submit'}
      </button>
    </form>
  )
}
