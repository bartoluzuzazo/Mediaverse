import { createFileRoute } from '@tanstack/react-router'
import SongForm from '../../../../common/components/entries/songs/SongForm.tsx'

export const Route = createFileRoute('/entries/songs/create/')({
  component: () => <SongForm/>
})