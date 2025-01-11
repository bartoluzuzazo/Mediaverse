import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { EpisodeService } from '../../../../services/EntryServices/episodeService.ts'
import EntryAuthorPreview from '../AuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Episode } from '../../../../models/entry/episode/Episode.ts'
import EntryPreviewTile from '../EntryPreview/EntryPreviewTile.tsx'
import { ReviewsCarousel } from '../../reviews/reviewsCarousel'

interface EpisodeEntryComponentProps {
  id: string
}

const episodeQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_EPISODE', id],
    queryFn: async (): Promise<Episode> => {
      const res = await EpisodeService.getEpisode(id)
      return res.data
    },
  })
}

export const EpisodeEntryComponent: FunctionComponent<EpisodeEntryComponentProps> = ({id}) => {
  const episodeQuery = useSuspenseQuery(episodeQueryOptions(id))
  const episode = episodeQuery.data
  const info = [episode.entry.release.toString()]

  return (
    <>
      <EntryBanner entry={episode.entry} info={info} type={'Episode'} />
      <AuthorizedView allowedRoles="ContentCreator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/episodes/edit/$id'} params={{id: episode.entry.id}} icon={<FaPen/>}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={episode.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{episode.entry.description}</div>
      {episode.entry.authors.map((group) => (
        <Fragment key={group.role}>
          <SectionHeader title={group.role} />
          <div className="flex flex-wrap">
            {group.authors.map((a) => (
              <div className="p-2" key={a.id}>
                <EntryAuthorPreview author={a} />
              </div>
            ))}
          </div>
        </Fragment>
      ))}
      <SectionHeader title={'Series'} />
      <EntryPreviewTile entry={episode.series}/>
      <ReviewsCarousel entryId={id} />
      <CommentSection entryId={id} />
    </>
  )
}