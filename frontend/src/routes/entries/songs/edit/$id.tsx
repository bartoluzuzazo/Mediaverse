import { createFileRoute } from '@tanstack/react-router'
import { SongService } from '../../../../services/EntryServices/songService.ts'
import { Song } from '../../../../models/entry/song/Song.ts'
import SongForm from '../../../../common/components/entries/songs/SongForm.tsx'

export const Route = createFileRoute('/entries/songs/edit/$id')({
  loader: async ({ params }) => {
    const response = await SongService.getSong(params.id)
    return response.data
  },
  component: () => {
    const song = Route.useLoaderData<Song>()
    return song && <SongForm song={song} />
  },
})