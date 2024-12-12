import { createFileRoute } from '@tanstack/react-router'
import { SeriesService } from '../../../../services/EntryServices/seriesService.ts'
import { Series } from '../../../../models/entry/series/Series.ts'
import SeriesForm from '../../../../common/components/entries/series/SeriesForm.tsx'

export const Route = createFileRoute('/entries/series/edit/$id')({
  loader: async ({ params }) => {
    const response = await SeriesService.getSeries(params.id)
    return response.data
  },
  component: () => {
    const series = Route.useLoaderData<Series>()
    return series && <SeriesForm series={series} />
  },
})