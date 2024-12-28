import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Series } from '../../../../models/entry/series/Series.ts'
import { Fragment, FunctionComponent, useState } from 'react'
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
import SectionHeader from '../sectionHeader.tsx'
import SeriesEpisodePreview from './SeriesEpisodePreview.tsx'
import { LinkButton } from '../../shared/LinkButton'
import { IoAddSharp } from 'react-icons/io5'
import { Modal } from '../../shared/Modal'
import ModalEpisodeForm, { EpisodeFormData } from '../episodes/ModalEpisodeForm.tsx'
import { Episode, EpisodePreview } from '../../../../models/entry/episode/Episode.ts'
import { EpisodeService } from '../../../../services/EntryServices/episodeService.ts'

export interface SeriesFormData {
  entry: Entry,
  genres: string[],
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
      }
      : undefined,
  })

  const navigate = useNavigate()
  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())
  const [isOpen, setIsOpen] = useState<boolean>(false)
  const [edited, setEdited] = useState<Episode>()
  const [episodes, setEpisodes] = useState<EpisodeFormData[]>([])

  const handleEdit = async (ep: EpisodePreview) => {
    const episode = await EpisodeService.getEpisode(ep.id)
    setEdited(episode.data);
    setIsOpen(true);
  }

  const handleAdd = async () => {
    setEdited(undefined);
    setIsOpen(true);
  }

  const onEpisodeSubmit: SubmitHandler<EpisodeFormData> = async (data) => {
    if (edited === undefined){
      data.entry.workOnRequests = authors;
      setEpisodes([...episodes, data]);
    }
    else{
    }
    setIsOpen(false)
  }

  const onSubmit: SubmitHandler<SeriesFormData> = async (data) => {
    data.genres = genres
    data.entry.workOnRequests = authors;
    console.log(data)
    if (series == null) {
      const response = await SeriesService.postSeries(data);
      const id = response.data.id;
      const epResponse = await EpisodeService.postEpisodes(id, episodes);
      console.log(epResponse.data)
      await navigate({ to: `/entries/${id}` })
    } else {
      await SeriesService.patchSeries(data, series.entry.id);
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
          <FormTextArea label={'Description'} register={register} errorValue={errors.entry?.description} registerPath={'entry.description'} />
        </div>
        <div className="flex-1 md:ml-20">
          <div>
            {series?.seasons.map((season) => (
              <Fragment key={season.seasonNumber}>
                <div className="flex flex-row justify-center">
                  <div>
                    <SectionHeader title={`Season ${season.seasonNumber}`} />
                    {season.episodes.map((ep) => (
                      <div className="p-2" key={ep.id}>
                        <SeriesEpisodePreview episode={ep} onClick={() => handleEdit(ep)} />
                      </div>
                    ))}
                  </div>
                </div>
              </Fragment>
            ))}
          </div>
          <div>
            {episodes.map((ep) => (
              <div className="p-2">
                <SeriesEpisodePreview episode={{
                  id: "",
                  name: ep.entry.name,
                  episodeNumber: ep.episodeNumber,
                  release: ep.entry.release,
                  ratingAvg: 0,
                  synopsis: ep.synopsis
                }} onClick={() => handleEdit({
                  id: "",
                  name: ep.entry.name,
                  episodeNumber: ep.episodeNumber,
                  release: ep.entry.release,
                  ratingAvg: 0,
                  synopsis: ep.synopsis
                })} />
              </div>
            ))}
          </div>
          <div onClick={handleAdd}>
            <LinkButton icon={<IoAddSharp />}>
              Add Episodes
            </LinkButton>
          </div>
          <div className="flex flex-row-reverse pt-2">
            <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </FormButton>
          </div>
        </div>
      </form>
      <div className="flex flex-row justify-evenly">
        <GenreInputForm label={'Genres'} collection={genres} setCollection={setGenres}
                        searchFunction={GenresServices.searchCinematicGenres} />
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors} />
      </div>
      {isOpen && (
        <Modal onOutsideClick={() => setIsOpen(false)}>
          <ModalEpisodeForm episode={edited} onSubmit={onEpisodeSubmit} />
        </Modal>
      )}
    </>
  )
}

export default SeriesForm