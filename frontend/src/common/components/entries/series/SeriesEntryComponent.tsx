import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { SeriesService } from '../../../../services/EntryServices/seriesService.ts'
import EntryAuthorPreview from '../entryAuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Series } from '../../../../models/entry/series/Series.ts'
import SeriesEpisodePreview from './SeriesEpisodePreview.tsx'

interface SeriesEntryComponentProps {
  id: string
}

const seriesQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_SERIES', id],
    queryFn: async (): Promise<Series> => {
      const res = await SeriesService.getSeries(id)
      return res.data
    },
  })
}

export const SeriesEntryComponent: FunctionComponent<SeriesEntryComponentProps> = ({id}) => {
  const seriesQuery = useSuspenseQuery(seriesQueryOptions(id))
  const series = seriesQuery.data
  const info = [series.entry.release.toString(), ...series.cinematicGenres]

  return (
    <>
      <EntryBanner entry={series.entry} info={info} type={'Series'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/series/edit/$id'} params={{id: series.entry.id}} icon={<FaPen/>}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={series.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{series.entry.description}</div>
      {series.entry.authors.map((group) => (
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
      <SectionHeader title={'Episodes'} />
      {series.seasons.map((season) => (
        <Fragment key={season.seasonNumber}>
          <div className="flex flex-row justify-center">
            <div className="w-[95%]">
              <SectionHeader title={`Season ${season.seasonNumber}`} />
              {season.episodes.map((ep) => (
                <div className="p-2" key={ep.id}>
                  <SeriesEpisodePreview episode={ep} />
                </div>
              ))}
            </div>
          </div>
        </Fragment>
      ))}
      <div className="p-4">eps</div>
      <CommentSection entryId={id} />
    </>
  )
}