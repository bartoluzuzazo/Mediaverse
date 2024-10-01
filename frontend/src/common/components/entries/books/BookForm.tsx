import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Book, WorkOn } from '../../../../models/entry/book/Book.ts'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { BookService } from '../../../../services/bookService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { MultipleInputForm } from './MultipleInputForm.tsx'
import {  AuthorEntryInputForm } from './AuthorEntryInputForm.tsx'

export interface BookFormData {
  id?: string
  name: string,
  description: string,
  release: Date,
  coverPhoto: string,
  isbn: string,
  synopsis: string,
  genres: string[]
  workOnRequests: WorkOn[]
}

type Props = {
  book?: Book
}

const BookForm: FunctionComponent<Props> = ({ book }) => {

  const {
    register,
    handleSubmit,
    watch,
    control,
    getValues,
    formState: { errors, isSubmitting },
  } = useForm<BookFormData>({
    defaultValues: book
      ? {
        id: book.entry.id,
        name: book.entry.name,
        description: book.entry.description,
        synopsis: book.synopsis,
        release: book.entry.release,
        isbn: book.isbn,
        genres: book.bookGenres,
        workOnRequests: book.entry.authors.map(g => g.authors.map(a => {
          const workOn : WorkOn = {id: a.id, role: g.role}
          return workOn
        })).flat()
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getValues('workOnRequests')?getValues('workOnRequests'):[])

  const onSubmit: SubmitHandler<BookFormData> = async (data) => {
    data.genres = genres
    data.workOnRequests = authors;

    if (book == null) {
      console.log(data)
      const response = await BookService.postBook(data)
      const id = response.data.id
      await navigate({ to: `/entries/books/${id}` })
    } else {
      await BookService.patchBook(data, book.entry.id)
      await navigate({ to: `/entries/books/${book.entry.id}` })
    }
  }

  return (
    <>
      <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800 md:h-32"></div>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-col p-4 md:flex-row"
      >
        <div>
          <CoverPicker<BookFormData> control={control} name={'coverPhoto'} watch={watch} previousImageSrc={book?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.name} registerPath={'name'} />
          <FormField label={'ISBN'} register={register} errorValue={errors.isbn} registerPath={'isbn'} />
          <FormDateInput label={'Release'} register={register} errorValue={errors.release} registerPath={'release'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Description'} register={register} errorValue={errors.description} registerPath={'description'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Synopsis'} register={register} errorValue={errors.isbn} registerPath={'synopsis'}/>
          <div className="flex flex-row-reverse">
            <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </FormButton>
          </div>
        </div>
      </form>
      <div className="flex flex-row justify-evenly">
        <MultipleInputForm label={'Genres'} collection={genres} setCollection={setGenres}/>
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
      </div>
    </>
  )
}

export default BookForm