import { Controller, FieldValues, Path, UseFormReturn } from 'react-hook-form'
import MarkdownEditor from '@uiw/react-markdown-editor'

type MarkdownFieldProps<T extends FieldValues> = {
  control: UseFormReturn<T>['control']
  name: Path<T>
  minHeight?: string
}

export const MarkdownField = <T extends FieldValues>({
  control,
  name,
  minHeight = '300px',
}: MarkdownFieldProps<T>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field: { value, onChange } }) => {
        return (
          <MarkdownEditor
            value={value}
            onChange={onChange}
            minHeight={minHeight}
          />
        )
      }}
    />
  )
}
