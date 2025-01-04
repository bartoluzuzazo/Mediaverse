import { createFileRoute } from '@tanstack/react-router'
import SeriesForm from '../../../../common/components/entries/series/SeriesForm.tsx'

export const Route = createFileRoute('/entries/series/create/')({
  component: () => <SeriesForm/>
})