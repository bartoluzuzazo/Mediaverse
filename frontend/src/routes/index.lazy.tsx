import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const [topBooks, newestBooks, topMovies, newestMovies, topGames, newestGames] = Route.useLoaderData()

  return (
    <div className="mt-10 flex flex-col gap-5">
      <EntryCarousel
        entries={topBooks?.data.entries || []}
        title="Top Books"
      />
      <EntryCarousel
        entries={newestBooks?.data.entries || []}
        title="Newest Books"
      />
      <EntryCarousel
        entries={topMovies?.data.entries || []}
        title="Top Movies"
      />
      <EntryCarousel
        entries={newestMovies?.data.entries || []}
        title="Newest Movies"
      />
      <EntryCarousel
        entries={topGames?.data.entries || []}
        title="Top Games"
      />
      <EntryCarousel
        entries={newestGames?.data.entries || []}
        title="Newest Games"
      />
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
