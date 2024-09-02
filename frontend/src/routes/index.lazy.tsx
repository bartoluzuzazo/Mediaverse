import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const [topBooks, newestBooks] = Route.useLoaderData()

  return (
    <div className="mt-10 flex flex-col gap-5">
      <EntryCarousel
        entries={topBooks?.data.books || []}
        title="Top Books This Month"
      />
      <EntryCarousel
        entries={newestBooks?.data.books || []}
        title="Newest Books This Month"
      />
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
