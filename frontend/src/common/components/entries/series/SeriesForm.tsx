import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Series } from '../../../../models/entry/series/Series.ts'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { SeriesService } from '../../../../services/EntryServices/seriesService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { GenreInputForm } from '../GenreInputForm.tsx'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'
import { GenresServices } from '../../../../services/EntryServices/genresServices.ts'
import { EpisodeService } from '../../../../services/EntryServices/episodeService.ts'
import { EntryFormPreview, SearchEntryForm } from '../albums/SearchEntryForm.tsx'

export interface SeriesFormData {
  entry: Entry
  genres: string[]
  episodeIds: string[]
}

type Props = {
  series?: Series
}

const SeriesForm: FunctionComponent<Props> = ({ series }) => {

  const getInitialWorkOns = () => {
    if (series === undefined){
      return []
    }

    return series!.entry.authors.flatMap(g => g.authors.map(a => {
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
  } = useForm<SeriesFormData>({
    defaultValues: series
      ? {
        entry : series.entry,
        genres: series.cinematicGenres,
        episodeIds: series.seasons.flatMap(s => s.episodes).map(e => e.id)
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())
  const [episodes, setEpisodes] = useState<EntryFormPreview[]>(series ? series.seasons.flatMap(s => s.episodes).map(e => {
    let preview : EntryFormPreview = {id: e.id, name: e.name}
    return preview
  }) : [])

  const onSubmit: SubmitHandler<SeriesFormData> = async (data) => {
    data.genres = genres;
    data.entry.workOnRequests = authors;
    data.episodeIds = episodes.map(s => s.id);
    if (series == null) {
      const response = await SeriesService.postSeries(data)
      const id = response.data.id
      await navigate({ to: `/entries/${id}` })
    } else {
      await SeriesService.patchSeries(data, series.entry.id)
      await navigate({ to: `/entries/${series.entry.id}` })
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
          <CoverPicker<SeriesFormData> control={control} name={'entry.photo'} watch={watch} previousImageSrc={series?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.entry?.name} registerPath={'entry.name'} />
          <FormDateInput label={'Release'} register={register} errorValue={errors.entry?.release} registerPath={'entry.release'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Description'} register={register} errorValue={errors.entry?.description}
                        registerPath={'entry.description'} rows={10}/>
          <div className="flex-1 md:ml-20">
            <div className="flex flex-row-reverse">
              <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
                {isSubmitting ? 'Submitting...' : 'Submit'}
              </FormButton>
            </div>
          </div>
        </div>

      </form>
      <div className="flex flex-row justify-evenly">
        <GenreInputForm label={'Genres'} collection={genres} setCollection={setGenres}
                        searchFunction={GenresServices.searchCinematicGenres} />
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
        <SearchEntryForm label={'Episodes'} collection={episodes} setCollection={setEpisodes} searchFunction={EpisodeService.search} queryKey={"SEARCH_SONGS"}/>
      </div>
    </>
  )
}

export default SeriesForm