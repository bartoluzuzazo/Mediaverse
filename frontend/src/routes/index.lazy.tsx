import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const loaderData = Route.useLoaderData()

  return (
    <div className="mt-10 flex flex-col gap-5">
      {loaderData.map(d => {
        return <EntryCarousel
          entries={d?.data.data.entries || []}
          title={d?.title}
        />
      })}
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
