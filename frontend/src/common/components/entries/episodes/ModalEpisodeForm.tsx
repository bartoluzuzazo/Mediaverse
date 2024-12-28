import FormButton from '../../form/button'
import { SubmitHandler, useForm } from 'react-hook-form'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'
import { Episode } from '../../../../models/entry/episode/Episode.ts'
import { IoIosWarning } from 'react-icons/io'

export interface EpisodeFormData {
  entry: Entry,
  synopsis: string,
  seasonNumber: number,
  episodeNumber: number,
}

type Props = {
  episode?: Episode
  onSubmit: SubmitHandler<EpisodeFormData>
}

const ModalEpisodeForm: FunctionComponent<Props> = ({ episode, onSubmit }) => {

  const getInitialWorkOns = () => {
    if (episode === undefined){
      return []
    }

    return episode!.entry.authors.flatMap(g => g.authors.map(a => {
      const workOn : WorkOn = {id: a.id, name: `${a.name} ${a.surname}`, role: g.role, details: a.details}
      return workOn
    }))
  }

  const {
    register,
    handleSubmit,
    watch,
    control,
    formState: { errors, isSubmitting },
  } = useForm<EpisodeFormData>({
    defaultValues: episode
      ? {
        seasonNumber: episode.seasonNumber,
        episodeNumber: episode.episodeNumber,
        entry : episode.entry,
        synopsis: episode.synopsis,
      }
      : undefined
  })

  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())


  return (
    <>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-col p-4 md:flex-row"
      >
        <div>
          <CoverPicker<EpisodeFormData> control={control} name={'entry.photo'} watch={watch}
                                        previousImageSrc={episode?.entry.photo} />
          <FormField label={'Name'} register={register} errorValue={errors.entry?.name} registerPath={'entry.name'} />
          <FormDateInput label={'Release'} register={register} errorValue={errors.entry?.release}
                         registerPath={'entry.release'} />
          <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors} />
        </div>
        <div className="flex-1 md:ml-20">
          <div className="flex flex-row justify-between pb-3">
            <label>
              Season
              <input type="number" min="1" className="block w-20 rounded-md border-2 border-slate-500 p-1"
                     {...register('seasonNumber', { required: 'Season number is required' })}
              />
              {errors.seasonNumber && (
                <div className="text-red-700 flex flex-row">
                  <IoIosWarning />
                  <div>{errors.seasonNumber.message}</div>
                </div>
              )}
            </label>
            <label>
              Episode
              <input type="number" min="1" className="block w-20 rounded-md border-2 border-slate-500 p-1"
                     {...register('episodeNumber', { required: 'Episode number is required' })}
              />
              {errors.episodeNumber && (
                <div className="text-red-700 flex flex-row">
                  <IoIosWarning />
                  <div>{errors.episodeNumber.message}</div>
                </div>
              )}
            </label>
          </div>
          <FormTextArea label={'Description'} register={register} errorValue={errors.entry?.description}
                        registerPath={'entry.description'} rows={15} />
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
    </>
  )
}

export default ModalEpisodeForm