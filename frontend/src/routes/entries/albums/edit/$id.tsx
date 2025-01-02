import { createFileRoute } from '@tanstack/react-router'
import { Song } from '../../../../models/entry/song/Song.ts'
import AlbumForm from '../../../../common/components/entries/albums/AlbumForm.tsx'
import { AlbumService } from '../../../../services/EntryServices/albumService.ts'

export const Route = createFileRoute('/entries/albums/edit/$id')({
  loader: async ({ params }) => {
    const response = await AlbumService.getAlbum(params.id)
    return response.data
  },
  component: () => {
    const album = Route.useLoaderData<Song>()
    return album && <AlbumForm album={album} />
  },
})