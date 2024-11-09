import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Movie } from '../../../../models/entry/movie/Movie.ts'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { MovieService } from '../../../../services/movieService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { MultipleInputForm } from '../MultipleInputForm.tsx'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'

export interface MovieFormData {
  entry: Entry
  synopsis: string,
  genres: string[]
}

type Props = {
  movie?: Movie
}

const MovieForm: FunctionComponent<Props> = ({ movie }) => {

  const getInitialWorkOns = () => {
    if (movie === undefined){
      return []
    }

    return movie!.entry.authors.flatMap(g => g.authors.map(a => {
      const workOn : WorkOn = {id: a.id, name: `${a.name} ${a.surname}`, role: g.role}
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
  } = useForm<MovieFormData>({
    defaultValues: movie
      ? {
        entry : movie.entry,
        genres: movie.cinematicGenres,
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())

  const onSubmit: SubmitHandler<MovieFormData> = async (data) => {
    data.genres = genres
    data.entry.workOnRequests = authors;

    if (movie == null) {
      const response = await MovieService.postMovie(data)
      const id = response.data.id
      await navigate({ to: `/entries/movies/${id}` })
    } else {
      await MovieService.patchMovie(data, movie.entry.id)
      await navigate({ to: `/entries/movies/${movie.entry.id}` })
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
          <CoverPicker<MovieFormData> control={control} name={'entry.photo'} watch={watch} previousImageSrc={movie?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.entry?.name} registerPath={'entry.name'} />
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
        <MultipleInputForm label={'Genres'} collection={genres} setCollection={setGenres}/>
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
      </div>
    </>
  )
}

export default MovieForm