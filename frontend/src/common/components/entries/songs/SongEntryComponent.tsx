import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { SongService } from '../../../../services/EntryServices/songService.ts'
import EntryAuthorPreview from '../AuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Song } from '../../../../models/entry/song/Song.ts'
import EntryPreviewTile from '../EntryPreview/EntryPreviewTile.tsx'

interface SongEntryComponentProps {
  id: string
}

const songQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_SONG', id],
    queryFn: async (): Promise<Song> => {
      const res = await SongService.getSong(id)
      return res.data
    },
  })
}

export const SongEntryComponent: FunctionComponent<SongEntryComponentProps> = ({id}) => {
  const songQuery = useSuspenseQuery(songQueryOptions(id))
  const song = songQuery.data
  const info = [song.entry.release.toString(), ...song.musicGenres]

  return (
    <>
      <EntryBanner entry={song.entry} info={info} type={'Song'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/songs/edit/$id'} params={{ id: song.entry.id }} icon={<FaPen />}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={song.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{song.entry.description}</div>
      <SectionHeader title={'Albums'} />
      {song.albums?.map(album => <EntryPreviewTile entry={album} />)}
      {song.entry.authors.map((group) => (
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
      <SectionHeader title={'Lyrics'} />
      <div className="p-4 whitespace-pre-wrap">{song.lyrics}</div>
      <CommentSection entryId={id} />
    </>
  )
}