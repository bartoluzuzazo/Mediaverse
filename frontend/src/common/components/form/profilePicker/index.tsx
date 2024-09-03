import { FieldValues, Path, UseFormReturn } from 'react-hook-form'
import defaultImgUrl from '/person-icon.png'
import ImagePicker from '../imagePicker'

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
        <ImagePicker<T>
          control={control}
          name={name}
          makeButton={(onClick) => {
            return (
              <button
                onClick={onClick}
                className="mx-auto block border-2 border-black p-2 bg-white"
                type="button"
              >
                Upload photo
              </button>
            )
          }}
        />
      </div>
    </div>
  )
}
export default ProfilePicker
