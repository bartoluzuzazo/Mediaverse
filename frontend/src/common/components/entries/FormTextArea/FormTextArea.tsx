import { IoIosWarning } from 'react-icons/io'
import {
  FieldError,
  FieldValues,
  Path,
  RegisterOptions,
  UseFormRegister,
} from 'react-hook-form'

interface Props<T extends FieldValues> {
  label: string
  registerPath: Path<T>
  register: UseFormRegister<T>
  errorValue: FieldError | undefined
  rows?: number
  rules?: RegisterOptions<T>
}

const FormTextArea = <T extends FieldValues>({
  label,
  registerPath,
  register,
  errorValue,
  rows = 14,
  rules = { required: `${label} is required` },
}: Props<T>) => {
  return (
    <div className="mb-2 block">
      <label>
        {label}
        <textarea
          {...register(registerPath, rules)}
          className="block w-full rounded-md border-2 border-slate-500 p-1"
          rows={rows}
        />
      </label>
      {errorValue && (
        <div className="flex flex-row text-red-700">
          <IoIosWarning />
          <div>{errorValue.message}</div>
        </div>
      )}
    </div>
  )
}

export default FormTextArea
