import { createFileRoute } from '@tanstack/react-router'
import AlbumForm from '../../../../common/components/entries/albums/AlbumForm.tsx'
import { AlbumService } from '../../../../services/EntryServices/albumService.ts'
import { Album } from '../../../../models/entry/album/Album.ts'

export const Route = createFileRoute('/entries/albums/edit/$id')({
  loader: async ({ params }) => {
    const response = await AlbumService.getAlbum(params.id)
    return response.data
  },
  component: () => {
    const album = Route.useLoaderData<Album>()
    return album && <AlbumForm album={album} />
  },
})