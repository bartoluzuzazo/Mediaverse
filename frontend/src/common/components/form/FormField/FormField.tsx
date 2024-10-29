import { IoIosWarning } from 'react-icons/io'
import { FieldError, FieldValues, Path, RegisterOptions, UseFormRegister } from 'react-hook-form'
import { HTMLInputTypeAttribute } from 'react'

interface Props<T extends FieldValues> {
  label: string
  registerPath: Path<T>
  register: UseFormRegister<T>
  errorValue?: FieldError
  type?: HTMLInputTypeAttribute
  rules?: RegisterOptions<T>
  disabled?: boolean
}

const FormField = <T extends FieldValues>({
                                            label,
                                            registerPath,
                                            register,
                                            errorValue,
                                            type = 'text',
                                            rules = { required: `${label} is Required` },
                                            disabled = false,
                                          }: Props<T>) => {
  return (
    <div className="mb-2 block">
      <label>
        {label}
        <input disabled={disabled}
               {
                 ...register(registerPath, rules)
               }
               className="block w-full rounded-md border-2 border-slate-500 p-1"
               type={type}
        />
      </label>
      {errorValue && (
        <div className="text-red-700 flex flex-row">
          <IoIosWarning />
          <div>{errorValue.message}</div>
        </div>
      )}
    </div>
  )
}
export default FormField
