import {
  Controller,
  FieldError,
  FieldValues,
  Path,
  UseFormReturn,
} from 'react-hook-form'
import RatingPicker from '../../entryRatingPicker/RatingPicker.tsx'
import { IoIosWarning } from 'react-icons/io'

type Props<T extends FieldValues> = {
  control: UseFormReturn<T>['control']
  name: Path<T>
  errorValue?: FieldError
}

export const RatingField = <T extends FieldValues>({
  control,
  name,
  errorValue,
}: Props<T>) => {
  return (
    <div>
      <Controller
        name={name}
        control={control}
        rules={{ required: 'Grade is required' }}
        render={({ field: { value, onChange } }) => {
          return <RatingPicker onClick={onChange} previousGrade={value} />
        }}
      />
      {errorValue && (
        <div className="flex flex-row text-red-700">
          <IoIosWarning />
          <div>{errorValue.message}</div>
        </div>
      )}
    </div>
  )
}
