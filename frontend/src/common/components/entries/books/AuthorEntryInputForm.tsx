import { useForm } from 'react-hook-form'
import { Dispatch, FunctionComponent, SetStateAction, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { WorkOn } from '../../../../models/entry/book/Book.ts'
import { DeletableGrid } from '../../form/deletableGrid/DeletableGrid.tsx'
import { Modal } from '../../shared/Modal'
import { UserSearch } from '../../users/UserSearch'
import { FaMagnifyingGlass } from 'react-icons/fa6'
import { AuthorService } from '../../../../services/AuthorService.ts'
import { Author } from '../../../../models/author/Author.ts'
import { User } from '../../../../models/user'

interface Props {
  label: string
  collection: WorkOn[]
  setCollection: Dispatch<SetStateAction<WorkOn[]>>
}

export const AuthorEntryInputForm: FunctionComponent<Props> = ({ label, collection, setCollection }) => {

  const form = useForm<WorkOn>()
  const handleAdd = (data: WorkOn) => {
    setCollection((prev) => [...prev, data])
  }

  const [isOpen, setIsOpen] = useState<boolean>(false)

  const addAuthor = (u: User | Author) => {
    if ('name' in u) {
      form.setValue('id', u.id)
      form.setValue('name', `${u.name} ${u.surname}`)
    }
  }

  const displayFn = (wo: WorkOn) => {
    return `${wo.name} - ${wo.role}`
  }

  return (
    <div className="flex flex-col">
      <DeletableGrid label={label} collection={collection} setCollection={setCollection} displayFn={displayFn} />
      <button className="mt-2 bg-violet-200 flex items-center rounded-md gap-3 p-1 w-full border-none hover:scale-105"
              onClick={() => setIsOpen(true)}>
        <div
          className="self-center text-white bg-violet-900 rounded-full w-10 aspect-square m-1 grid place-content-center">
          <FaMagnifyingGlass className="text-xl" />
        </div>
        <span className="font-semibold text-lg">Search Author</span>
      </button>
      {isOpen &&
        <Modal onOutsideClick={() => setIsOpen(false)}>
          <form id="authorForm"
                onSubmit={form.handleSubmit(handleAdd)}
                className="flex flex-col pt-2"
          >
            <FormField label={'ID'} register={form.register} errorValue={form.formState.errors.id}
                       registerPath={'id'} disabled={true} />
            <FormField label={'Name'} register={form.register} errorValue={form.formState.errors.name}
                       registerPath={'name'} disabled={true} />
            <label>Search authors using name or surname</label>
            <UserSearch service={AuthorService} onClick={async (u) => addAuthor(u) } queryKey="SEARCH_AUTHOR"/>
            <FormField label={'Role'} register={form.register} errorValue={form.formState.errors.role}
                       registerPath={'role'} />
            <button
              className="h-[36px] w-[36px] text-white bg-mv-light-purple flex items-center justify-center mb-2 p-1">
              +
            </button>
          </form>
        </Modal>}
      <div className="mb-2 flex flex-row items-end">
      </div>
    </div>
  )
}