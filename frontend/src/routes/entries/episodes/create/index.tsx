import { createFileRoute } from '@tanstack/react-router'
import EpisodeForm from '../../../../common/components/entries/episodes/EpisodeForm.tsx'

export const Route = createFileRoute('/entries/episodes/create/')({
  component: () => <EpisodeForm/>
})