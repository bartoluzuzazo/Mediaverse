import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { AlbumService } from '../../../../services/EntryServices/albumService.ts'
import EntryAuthorPreview from '../entryAuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Album } from '../../../../models/entry/album/Album.ts'
import AuthorEntryPreview from '../EntryPreview/AuthorEntryPreview.tsx'

interface AlbumEntryComponentProps {
  id: string
}

const albumQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_ALBUM', id],
    queryFn: async (): Promise<Album> => {
      const res = await AlbumService.getAlbum(id)
      return res.data
    },
  })
}

export const AlbumEntryComponent: FunctionComponent<AlbumEntryComponentProps> = ({id}) => {
  const albumQuery = useSuspenseQuery(albumQueryOptions(id))
  const album = albumQuery.data
  const info = [album.entry.release.toString(), ...album.musicGenres]

  return (
    <>
      <EntryBanner entry={album.entry} info={info} type={'Album'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/albums/edit/$id'} params={{id: album.entry.id}} icon={<FaPen/>}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={album.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{album.entry.description}</div>
      <SectionHeader title={'Songs'} />
      {album.songs?.map(song => <AuthorEntryPreview entry={song}/>)}
      {album.entry.authors.map((group) => (
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
      <CommentSection entryId={id} />
    </>
  )
}