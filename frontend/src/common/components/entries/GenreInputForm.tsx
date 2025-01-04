import { useForm } from 'react-hook-form'
import { Dispatch, FunctionComponent, SetStateAction, useState } from 'react'
import { DeletableGrid } from '../form/deletableGrid/DeletableGrid.tsx'
import { GenrePicker } from './GenrePicker.tsx'
import { Page, PaginateRequest } from '../../../models/common'
import { AxiosResponse } from 'axios'

interface FormFieldData {
  field: string
}

interface Props {
  label: string
  collection: string[]
  setCollection: Dispatch<SetStateAction<string[]>>
  searchFunction: (query: string, params: PaginateRequest) => Promise<AxiosResponse<Page<string>>>
}

export const GenreInputForm: FunctionComponent<Props> = ({ label, collection, setCollection, searchFunction }) => {
  const [query, setQuery] = useState<string>('')

  const form = useForm<FormFieldData>()
  const handleAdd = () => {
    if (query !== '' && !collection.includes(query)) {
      setCollection((prev) => [...prev, query])
    }
  }

  function handleFill(g: string) {
    if (!collection.includes(g)) {
      setCollection((prev) => [...prev, g])
    }
  }

  return (
    <div className="flex flex-col">
      <DeletableGrid label={label} collection={collection} setCollection={setCollection} />
      <form id="genreForm"
            onSubmit={form.handleSubmit(() => {
            })}
            className="flex md:flex-row pt-2"
      >
        <div className="mb-2 flex flex-row">
          <GenrePicker
            query={query} setQuery={setQuery}
            searchFunction={searchFunction}
            onClick={async (u) => handleFill(u)}
            queryKey={searchFunction.name.toUpperCase()}
          />
          <button className="h-[36px] w-[36px] text-white bg-mv-light-purple flex items-center justify-center mb-2 p-5"
                  onClick={handleAdd}>
            +
          </button>
        </div>
      </form>
    </div>
  )
}