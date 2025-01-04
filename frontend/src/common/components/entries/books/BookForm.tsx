import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Book } from '../../../../models/entry/book/Book.ts'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { BookService } from '../../../../services/EntryServices/bookService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { GenreInputForm } from '../GenreInputForm.tsx'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { GenresServices } from '../../../../services/EntryServices/genresServices.ts'

export interface BookFormData {
  entry: Entry
  isbn: string,
  synopsis: string,
  genres: string[]
}

type Props = {
  book?: Book
}

const BookForm: FunctionComponent<Props> = ({ book }) => {

  const getInitialWorkOns = () => {
    if (book === undefined){
      return []
    }

    return book!.entry.authors.flatMap(g => g.authors.map(a => {
      const workOn : WorkOn = {id: a.id, name: `${a.name} ${a.surname}`, role: g.role, details: a.details}
      return workOn
    }))
  }

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
        entry : book.entry,
        isbn: book.isbn,
        genres: book.bookGenres,
        synopsis: book.synopsis
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())

  const onSubmit: SubmitHandler<BookFormData> = async (data) => {
    data.genres = genres
    data.entry.workOnRequests = authors;
    console.log(data)
    if (book == null) {
      const response = await BookService.postBook(data)
      const id = response.data.id
      await navigate({ to: `/entries/${id}` })
    } else {
      await BookService.patchBook(data, book.entry.id)
      await navigate({ to: `/entries/${book.entry.id}` })
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
          <CoverPicker<BookFormData> control={control} name={'entry.photo'} watch={watch} previousImageSrc={book?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.entry?.name} registerPath={'entry.name'} />
          <FormField label={'ISBN'} register={register} errorValue={errors.isbn} registerPath={'isbn'} />
          <FormDateInput label={'Release'} register={register} errorValue={errors.entry?.release} registerPath={'entry.release'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Description'} register={register} errorValue={errors.entry?.description} registerPath={'entry.description'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Synopsis'} register={register} errorValue={errors.synopsis} registerPath={'synopsis'}/>
          <div className="flex flex-row-reverse">
            <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </FormButton>
          </div>
        </div>
      </form>
      <div className="flex flex-row justify-evenly">
        <GenreInputForm label={'Genres'} collection={genres} setCollection={setGenres} searchFunction={GenresServices.searchBookGenres}/>
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
      </div>
    </>
  )
}

export default BookForm