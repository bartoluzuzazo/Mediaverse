import { IoIosWarning } from 'react-icons/io'
import { FieldError, FieldValues, Path, UseFormRegister } from 'react-hook-form'

interface Props <T extends FieldValues>{
  label: string
  registerPath: Path<T>
  register:  UseFormRegister<T>
  errorValue: FieldError | undefined
}

const FormDateInput = <T extends FieldValues> ({label, registerPath, register, errorValue} : Props<T>) => {
  return (
    <div className="mb-2 block">
      <label>
        {label}
        <input
          {
            ...register(registerPath, { required: `${label} is required` })
          }
          className="block w-full rounded-md border-2 border-slate-500 p-1"
          type="date"
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

export default FormDateInput;