import { useForm } from 'react-hook-form'
import { Dispatch, FunctionComponent, SetStateAction } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { DeletableGrid } from '../../form/deletableGrid/DeletableGrid.tsx'

interface FormFieldData {
  field: string
}

interface Props {
  label: string
  collection: string[]
  setCollection: Dispatch<SetStateAction<string[]>>
}

export const MultipleInputForm : FunctionComponent<Props> = ({label, collection, setCollection}) => {

  const form = useForm<FormFieldData>()
  const handleAdd = (data: FormFieldData) => {
    setCollection((prev) => [...prev, data.field])
    form.resetField('field')
  }

  return (
    <div className="flex flex-col">
      <DeletableGrid label={label} collection={collection} setCollection={setCollection}/>
      <form id="genreForm"
            onSubmit={form.handleSubmit(handleAdd)}
            className="flex md:flex-row pt-2"
      >
        <div className="mb-2 flex flex-row items-end">
          <FormField label={"Genre"} register={form.register} errorValue={form.formState.errors.field}
                     registerPath={'field'} />
            <button className='h-[36px] w-[36px] text-white bg-mv-light-purple flex items-center justify-center mb-2 p-1'>
              +
            </button>
          </div>
      </form>
    </div>
  )
}