import { IoIosWarning } from 'react-icons/io'
import { FieldError, FieldValues, Path, RegisterOptions, UseFormRegister } from 'react-hook-form'
import { HTMLInputTypeAttribute } from 'react'

interface Props<T extends FieldValues> {
  label: string
  name: Path<T>
  register: UseFormRegister<T>
  errorValue?: FieldError
  type?: HTMLInputTypeAttribute
  rules?: RegisterOptions<T>
}

const FormField = <T extends FieldValues>({
                                            label,
                                            name,
                                            register,
                                            errorValue,
                                            type = 'text',
                                            rules = { required: `${label} is Required` },
                                          }: Props<T>) => {
  return (
    <div className='mb-2 block'>
      <label>
        {label}
        <input
          {
            ...register(name, rules)
          }
          className='block w-full rounded-md border-2 border-slate-500 p-1'
          type={type}
        />
      </label>
      {errorValue && (
        <div className='text-red-700 flex flex-row'>
          <IoIosWarning />
          <div>{errorValue.message}</div>
        </div>
      )}
    </div>
  )
}
export default FormField
