import { createFileRoute } from '@tanstack/react-router'
import AlbumForm from '../../../../common/components/entries/albums/AlbumForm.tsx'

export const Route = createFileRoute('/entries/albums/create/')({
  component: () => <AlbumForm/>
})