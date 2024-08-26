import { Controller, FieldValues, Path, UseFormReturn } from 'react-hook-form'
import { useRef } from 'react'
import defaultImgUrl from '/person-icon.png'

type Props<T extends FieldValues> = {
  control: UseFormReturn<T>['control']
  name: Path<T>
  watch: UseFormReturn<T>['watch']
  previousImageSrc?: string
}

const ProfilePicker = <T extends FieldValues>({
  control,
  name,
  watch,
  previousImageSrc,
}: Props<T>) => {
  console.log(previousImageSrc)
  const fileInputRef = useRef<HTMLInputElement>(null)
  const image = watch(name)

  const prevImage =
    previousImageSrc && 'data:image/*;base64,' + previousImageSrc
  const curImage = image && 'data:image/*;base64,' + image
  return (
    <div className="relative w-40 md:w-52">
      <img
        src={curImage || prevImage || defaultImgUrl}
        className="-mt-16 aspect-square w-52 rounded-full border-4 border-white bg-slate-300 object-cover md:-mt-24 md:w-60"
        alt="profile picture"
      />

      <div className="absolute inset-0 flex items-center">
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
                    const promise = new Promise((resolve) => {
                      const reader = new FileReader()
                      reader.readAsDataURL(event.target.files![0])
                      reader.onload = () => {
                        const result = reader
                          .result!.toString()
                          .split('base64,')[1]
                        console.log(result)
                        resolve(result)
                      }
                    })
                    onChange(await promise)
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
