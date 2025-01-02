import { useForm } from 'react-hook-form'
import { Dispatch, FunctionComponent, SetStateAction, useState } from 'react'
import { FaMagnifyingGlass } from 'react-icons/fa6'
import { DeletableGrid } from '../../form/deletableGrid/DeletableGrid.tsx'
import { Modal } from '../../shared/Modal'
import FormField from '../../form/FormField/FormField.tsx'
import { SongService } from '../../../../services/EntryServices/songService.ts'
import { EntrySearchBar } from '../EntrySearchBar.tsx'
import { EntrySearch } from '../../../../models/entry/Entry.ts'

export interface SongFormPreview{
  id: string
  name: string
}

interface Props {
  label: string
  collection: SongFormPreview[]
  setCollection: Dispatch<SetStateAction<SongFormPreview[]>>
}

export const SearchSongForm: FunctionComponent<Props> = ({
                                                                 label,
                                                                 collection,
                                                                 setCollection,
                                                               }) => {
  const form = useForm<SongFormPreview>()
  const handleAdd = (data: SongFormPreview) => {
    if (collection.some(wo => wo.id === data.id)) return;
    setCollection((prev) => [...prev, data]);
  }

  const [isOpen, setIsOpen] = useState<boolean>(false)

  const addEntry = (e: EntrySearch) => {
      form.setValue('id', e.id)
      form.setValue('name', e.name)
  }

  const displayFn = (song: SongFormPreview) => {
    return song.name
  }

  return (
    <div className="flex flex-col">
      <DeletableGrid
        label={label}
        collection={collection}
        setCollection={setCollection}
        displayFn={displayFn}
      />
      <button
        className="mt-2 flex w-full items-center gap-3 rounded-md border-none bg-violet-200 p-1 hover:scale-105"
        onClick={() => setIsOpen(true)}
      >
        <div className="m-1 grid aspect-square w-10 place-content-center self-center rounded-full bg-violet-900 text-white">
          <FaMagnifyingGlass className="text-xl" />
        </div>
        <span className="text-lg font-semibold">Search Author</span>
      </button>
      {isOpen && (
        <Modal onOutsideClick={() => setIsOpen(false)}>
          <form
            id="authorForm"
            onSubmit={form.handleSubmit(handleAdd)}
            className="flex flex-col pt-2"
          >
            <FormField
              label={'ID'}
              register={form.register}
              errorValue={form.formState.errors.id}
              registerPath={'id'}
              disabled={true}
            />
            <FormField
              label={'Name'}
              register={form.register}
              errorValue={form.formState.errors.name}
              registerPath={'name'}
              disabled={true}
            />
            <label>Search authors using name or surname</label>
            <EntrySearchBar searchFunction={SongService.search} queryKey={"SEARCH_SONGS"} onClick={addEntry}/>
            <button className="mb-2 flex h-[36px] w-[36px] items-center justify-center bg-mv-light-purple p-1 text-white">
              +
            </button>
          </form>
        </Modal>
      )}
      <div className="mb-2 flex flex-row items-end"></div>
    </div>
  )
}
