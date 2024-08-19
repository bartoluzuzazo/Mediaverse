import { Controller, FieldValues, UseFormReturn } from 'react-hook-form'
import { useMemo, useRef } from 'react'
import defaultImgUrl from '/person-icon.png'

type Props<T extends FieldValues> = {
  control: UseFormReturn<T>['control']
  name: keyof T
  file: File
}

const ProfilePicker = <T extends FieldValues>({
  control,
  name,
  file,
}: Props<T>) => {
  const fileInputRef = useRef(null)

  const imageSrc = useMemo(() => {
    return file && URL.createObjectURL(file)
  }, [file])
  return (
    <div className="relative w-40 md:w-52">
      <img
        src={imageSrc || defaultImgUrl}
        className="-mt-16 aspect-square w-52 rounded-full border-4 border-white bg-slate-300 object-cover md:w-60"
        alt="profile picture"
      />

      <div className="absolute inset-0 flex items-center">
        <Controller
          control={control}
          name={name as never}
          render={({ field: { value: _value, onChange, ...field } }) => {
            return (
              <>
                <input
                  className="hidden"
                  {...field}
                  onChange={(event) => {
                    onChange(event.target.files![0])
                  }}
                  type={'file'}
                  ref={fileInputRef}
                />
                <button
                  className="mx-auto block border-2 border-black p-2"
                  type="button"
                  onClick={() => {
                    fileInputRef.current!.click()
                  }}
                >
                  Upload photo
                </button>
              </>
            )
          }}
        />
      </div>
    </div>
  )
}
export default ProfilePicker
