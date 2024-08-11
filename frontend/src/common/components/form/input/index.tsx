import { FunctionComponent, ReactElement } from 'react'

interface FormInputProps {
  inputProps?: React.DetailedHTMLProps<
    React.InputHTMLAttributes<HTMLInputElement>,
    HTMLInputElement
  >
  rightElement?: string | ReactElement
  leftElement?: string | ReactElement
}

const FormInput: FunctionComponent<FormInputProps> = ({
  inputProps,
  leftElement,
  rightElement,
}) => {
  return (
    <div className="border-mv-gray divide-mv-gray flex h-[45px] w-full flex-row divide-x overflow-hidden rounded-md border border-solid">
      {leftElement && (
        <span className="text-mv-gray flex items-center justify-center p-2">
          {leftElement}
        </span>
      )}

      <input className="flex-[1] border-none p-2" {...inputProps} />

      {rightElement && (
        <span className="text-mv-gray flex items-center justify-center p-2">
          {rightElement}
        </span>
      )}
    </div>
  )
}

export default FormInput
