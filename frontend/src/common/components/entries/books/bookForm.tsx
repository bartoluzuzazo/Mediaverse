import ProfilePicker from '../../form/profilePicker'
import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Book } from '../../../../models/entry/book/Book.ts'
import { FunctionComponent } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { BookService } from '../../../../services/bookService.ts'

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
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-col p-4 md:flex-row"
      >
        <div>
          <ProfilePicker<BookFormData>
            control={control}
            name={'coverPhoto'}
            watch={watch}
            previousImageSrc={book?.entry.photo}
          />

          <FormField label={'Name'} register={register} errorValue={errors.name} registerValue={'name'}/>
          <FormField label={'Description'} register={register} errorValue={errors.description} registerValue={'description'}/>
          <FormField label={'Release'} register={register} errorValue={errors.release} registerValue={'release'}/>
          <FormField label={'ISBN'} register={register} errorValue={errors.isbn} registerValue={'isbn'}/>
          <FormField label={'Synopsis'} register={register} errorValue={errors.isbn} registerValue={'synopsis'}/>

        </div>

        <div className="flex-1 md:ml-20">
          <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
            {isSubmitting ? 'Submitting...' : 'Submit'}
          </FormButton>
        </div>
      </form>
    </>
  )
}

export default bookForm