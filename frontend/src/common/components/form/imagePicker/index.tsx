import { Controller, FieldValues, Path, UseFormReturn } from 'react-hook-form'
import { ReactNode, useRef } from 'react'

type Props<T extends FieldValues> = {
  control: UseFormReturn<T>['control']
  name: Path<T>
  makeButton: (onClick: React.MouseEventHandler<HTMLButtonElement>) => ReactNode
}
const ImagePicker = <T extends FieldValues>({
  control,
  name,
  makeButton,
}: Props<T>) => {
  const fileInputRef = useRef<HTMLInputElement>(null)
  return (
    <Controller
      control={control}
      name={name}
      render={({ field: { value: _value, onChange, ...field } }) => {
        return (
          <>
            <input
              className="hidden"
              {...field}
              onChange={async (event) => {
                const reader = new FileReader()
                reader.readAsDataURL(event.target.files![0])
                reader.onload = () => {
                  const result = reader.result!.toString().split('base64,')[1]
                  onChange(result)
                }
              }}
              type={'file'}
              ref={fileInputRef}
            />
            {makeButton(() => {
              fileInputRef.current!.click()
            })}
          </>
        )
      }}
    />
  )
}

export default ImagePicker
