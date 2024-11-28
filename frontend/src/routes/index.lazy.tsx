import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const [topBooks, newestBooks, topMovies, newestMovies] = Route.useLoaderData()

  return (
    <div className="mt-10 flex flex-col gap-5">
      <EntryCarousel
        entries={topBooks?.data.entries || []}
        title="Top Books This Month"
      />
      <EntryCarousel
        entries={newestBooks?.data.entries || []}
        title="Newest Books This Month"
      />
      <EntryCarousel
        entries={topMovies?.data.entries || []}
        title="Top Movies This Month"
      />
      <EntryCarousel
        entries={newestMovies?.data.entries || []}
        title="Newest Movies This Month"
      />
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
