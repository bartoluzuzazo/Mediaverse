import FormButton from '../../form/button'
import { useNavigate } from '@tanstack/react-router'
import { SubmitHandler, useForm } from 'react-hook-form'
import { FunctionComponent, useState } from 'react'
import FormField from '../../form/FormField/FormField.tsx'
import { EpisodeService } from '../../../../services/EntryServices/episodeService.ts'
import FormTextArea from '../FormTextArea/FormTextArea.tsx'
import FormDateInput from '../../form/FormDateInput/FormDateInput.tsx'
import CoverPicker from '../../form/CoverPicker/CoverPicker.tsx'
import { Entry } from '../../../../models/entry/Entry.ts'
import { WorkOn } from '../../../../models/entry/WorkOn.ts'
import { AuthorEntryInputForm } from '../AuthorEntryInputForm.tsx'
import { Episode } from '../../../../models/entry/episode/Episode.ts'

export interface EpisodeFormData {
  entry: Entry
  synopsis: string,
}

type Props = {
  episode?: Episode
}

const EpisodeForm: FunctionComponent<Props> = ({ episode }) => {

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
        entry : episode.entry,
        synopsis: episode.synopsis
      }
      : undefined,
  })

  const navigate = useNavigate()

  const [authors, setAuthors] = useState<WorkOn[]>(getInitialWorkOns())

  const onSubmit: SubmitHandler<EpisodeFormData> = async (data) => {
    data.entry.workOnRequests = authors;

    if (episode == null) {
      const response = await EpisodeService.postEpisode(data)
      const id = response.data.id
      await navigate({ to: `/entries/${id}` })
    } else {
      await EpisodeService.patchEpisode(data, episode.entry.id)
      await navigate({ to: `/entries/${episode.entry.id}` })
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
          <CoverPicker<EpisodeFormData> control={control} name={'entry.photo'} watch={watch} previousImageSrc={episode?.entry.photo} />
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
        <AuthorEntryInputForm label={'Authors'} collection={authors} setCollection={setAuthors}/>
      </div>
    </>
  )
}

export default EpisodeForm