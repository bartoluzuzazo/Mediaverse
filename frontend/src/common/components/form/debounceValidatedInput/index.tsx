import { FieldValues, Path, UseFormReturn } from 'react-hook-form'
import { useCallback, useState } from 'react'
import { IoIosWarning } from 'react-icons/io'
import { useDebounce } from '../../../../hooks/useDebounce'

type Props<T extends FieldValues> = {
  formHookValues: UseFormReturn<T>
  validate: (value: string) => Promise<string | null>
  name: Path<T>
}

const DebounceValidatedInput = <T extends FieldValues>({
                                                         formHookValues,
                                                         validate,
                                                         name,
                                                       }: Props<T>) => {
  const [error, setError] = useState<string | null>(null)
  const [validatedValue, setValidatedValue] = useState<string>('')
  const { register, watch } = formHookValues

  const value = watch(name)

  const debounceHandler = useCallback(async () => {
    const error = await validate(value)
    setError(error)
    setValidatedValue(value)
  }, [value, validate])
  useDebounce(debounceHandler, 300)

  return (
    <div className='w-full'>
      <input
        {...register(name, {
          validate: () => error == null && validatedValue == value,
        })}
        className={`block w-full rounded-md border-2 p-1 ${validatedValue !== value ? 'border-slate-400' : error == null ? `border-green-700` : `border-red-700`}`}
        type='text'
      />
      {error && (
        <div className='flex flex-row text-red-700'>
          <IoIosWarning />
          <div>{error}</div>
        </div>
      )}
    </div>
  )
}

export default DebounceValidatedInput
