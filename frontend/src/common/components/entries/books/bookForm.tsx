import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Book } from '../../../../models/entry/book/Book.ts'
import { FunctionComponent } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { BookService } from '../../../../services/bookService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'

export interface BookFormData {
  id?: string
  name: string,
  description: string,
  release: Date,
  coverPhoto: string,
  isbn: string,
  synopsis: string,
  genreIds: string[]
}

type Props = {
  book?: Book
}

const bookForm: FunctionComponent<Props> = ({ book }) => {

  const {
    register,
    handleSubmit,
    watch,
    control,
    formState: { errors, isSubmitting },
  } = useForm<BookFormData>({
    defaultValues: book
      ? {
        id: book.id,
        name: book.entry.name,
        description: book.entry.description,
        release: book.entry.release,
        coverPhoto: book.entry.photo,
        isbn: book.isbn,
        genreIds: book.bookGenres,
      }
      : undefined,
  })

  const navigate = useNavigate()

  const onSubmit: SubmitHandler<BookFormData> = async (data) => {
    data.genreIds = []
    if (book == null) {
      const response = await BookService.postBook(data)
      const id = response.data.id
      await navigate({ to: `/entries/books/${id}` })
    } else {
      await BookService.patchBook(data, book.id)
      await navigate({ to: `/entries/books/${book.id}` })
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
    </>
  )
}

export default bookForm