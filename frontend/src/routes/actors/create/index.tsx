import { createFileRoute } from '@tanstack/react-router'
import { Control, Controller, SubmitHandler, useForm } from 'react-hook-form'
import FormButton from '../../../common/components/form/button'
import { useMemo, useRef } from 'react'
import defaultImgUrl from '/person-icon.png'

interface CreateAuthorCommand {
  name: string
  surname: string
  bio: string
  profilePicture: File
}

export const Route = createFileRoute('/actors/create/')({
  component: () => {
    const { register, handleSubmit, watch, control } =
      useForm<CreateAuthorCommand>()
    const file = watch('profilePicture')
    const fileInputRef = useRef(null)

    const imageSrc = useMemo(() => {
      return file && URL.createObjectURL(file)
    }, [file])
    console.log(file)
    const onSubmit: SubmitHandler<CreateAuthorCommand> = (data) => {
      console.log(data)
    }
    return (
      <>
        <div className="h-20 bg-violet-800"></div>

        <form onSubmit={handleSubmit(onSubmit)} className="p-4">
          <div className="relative w-40">
            <img
              src={imageSrc || defaultImgUrl}
              className="-mt-16 aspect-square w-40 rounded-full border-4 border-white bg-slate-300 object-cover"
            />

            <div className="absolute inset-0 flex items-center">
              <Controller
                control={control}
                name={'profilePicture'}
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
          <div className="mb-2 block">
            <label>
              Name
              <input
                {...register('name')}
                className="block h-8 w-full rounded-sm border-2 border-slate-500"
                type="text"
              />
            </label>
          </div>

          <div className="mb-2 block">
            <label>
              Surname
              <input
                {...register('surname')}
                className="block h-8 w-full rounded-sm border-2 border-slate-500"
                type="text"
              />
            </label>
          </div>

          <div className="mb-2 block">
            <label>
              Bio
              <textarea
                {...register('bio')}
                className="block w-full rounded-sm border-2 border-slate-500"
              />
            </label>
          </div>

          <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
            Submit
          </FormButton>
        </form>
      </>
    )
  },
})
