import { IoIosWarning } from 'react-icons/io'
import { FieldError, FieldValues, Path, UseFormRegister } from 'react-hook-form'

interface Props <T extends FieldValues>{
  label: string
  registerValue: Path<T>
  register:  UseFormRegister<T>
  errorValue: FieldError | undefined
}

const FormField = <T extends FieldValues> ({label, registerValue, register, errorValue} : Props<T>) => {
  return (
    <div className="mb-2 block">
      <label>
        {label}
        <input
          {
          ...register(registerValue, { required: `${label} is required` })
          }
          className="block w-full rounded-md border-2 border-slate-500 p-1"
          type="text"
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

export default FormField;