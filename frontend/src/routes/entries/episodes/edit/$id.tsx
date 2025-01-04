import { createFileRoute } from '@tanstack/react-router'
import EpisodeForm from '../../../../common/components/entries/episodes/EpisodeForm.tsx'
import { EpisodeService } from '../../../../services/EntryServices/episodeService.ts'
import { Episode } from '../../../../models/entry/episode/Episode.ts'

export const Route = createFileRoute('/entries/episodes/edit/$id')({
  loader: async ({ params }) => {
    const response = await EpisodeService.getEpisode(params.id)
    return response.data
  },
  component: () => {
    const episode = Route.useLoaderData<Episode>()
    return episode && <EpisodeForm episode={episode} />
  },
})