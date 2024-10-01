import { useForm } from 'react-hook-form'
import { Dispatch, FunctionComponent, SetStateAction } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { WorkOn } from '../../../../models/entry/book/Book.ts'
import { DeletableGrid } from '../../form/deletableGrid/DeletableGrid.tsx'

interface Props {
  label: string
  collection: WorkOn[]
  setCollection: Dispatch<SetStateAction<WorkOn[]>>
}

export const AuthorEntryInputForm: FunctionComponent<Props> = ({ label, collection, setCollection }) => {

  const form = useForm<WorkOn>()
  const handleAdd = (data: WorkOn) => {
    setCollection((prev) => [...prev, data])
    form.resetField('id')
    form.resetField('role')
  }

  const displayFn = (wo: WorkOn) => {
    return `${wo.id} - ${wo.role}`;
  }

  return (
    <div className="flex flex-col">
      <DeletableGrid label={label} collection={collection} setCollection={setCollection} displayFn={displayFn}/>
      <form id="genreForm"
            onSubmit={form.handleSubmit(handleAdd)}
            className="flex flex-col pt-2"
      >
        <FormField label={'Id [tu będzie wyszukiwarka]'} register={form.register} errorValue={form.formState.errors.id}
                   registerPath={'id'} />
        <div className="mb-2 flex flex-row items-end">

          <FormField label={'Role'} register={form.register} errorValue={form.formState.errors.role}
                     registerPath={'role'} />
          <button className="h-[36px] w-[36px] text-white bg-mv-light-purple flex items-center justify-center mb-2 p-1">
            +
          </button>
        </div>
      </form>
    </div>
  )
}