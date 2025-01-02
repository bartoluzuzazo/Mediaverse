import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Album } from '../../../../models/entry/album/Album.ts'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { AlbumService } from '../../../../services/EntryServices/albumService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { GenreInputForm } from '../GenreInputForm.tsx'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'
import { GenresServices } from '../../../../services/EntryServices/genresServices.ts'

export interface AlbumFormData {
  entry: Entry
  genres: string[]
}

type Props = {
  album?: Album
}

const AlbumForm: FunctionComponent<Props> = ({ album }) => {

  const getInitialWorkOns = () => {
    if (album === undefined){
      return []
    }

    return album!.entry.authors.flatMap(g => g.authors.map(a => {
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
  } = useForm<AlbumFormData>({
    defaultValues: album
      ? {
        entry : album.entry,
        genres: album.musicGenres,
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [genres, setGenres] = useState<string[]>(getValues('genres')?getValues('genres'):[])
  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())

  const onSubmit: SubmitHandler<AlbumFormData> = async (data) => {
    data.genres = genres
    data.entry.workOnRequests = authors;

    if (album == null) {
      const response = await AlbumService.postAlbum(data)
      const id = response.data.id
      await navigate({ to: `/entries/${id}` })
    } else {
      await AlbumService.patchAlbum(data, album.entry.id)
      await navigate({ to: `/entries/${album.entry.id}` })
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
          <CoverPicker<AlbumFormData> control={control} name={'entry.photo'} watch={watch} previousImageSrc={album?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.entry?.name} registerPath={'entry.name'} />
          <FormDateInput label={'Release'} register={register} errorValue={errors.entry?.release} registerPath={'entry.release'} />
        </div>
        <div className="flex-1 md:ml-20">
          <FormTextArea label={'Description'} register={register} errorValue={errors.entry?.description} registerPath={'entry.description'} />
        </div>
        <div className="flex-1 md:ml-20">
          <div className="flex flex-row-reverse">
            <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </FormButton>
          </div>
        </div>
      </form>
      <div className="flex flex-row justify-evenly">
        <GenreInputForm label={'Genres'} collection={genres} setCollection={setGenres} searchFunction={GenresServices.searchMusicGenres}/>
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
      </div>
    </>
  )
}

export default AlbumForm