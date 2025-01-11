import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'
import ArticleCarousel from '../common/components/articles/articleCarousel'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const { data, articles } = Route.useLoaderData()

  return (
    <div className="mt-10 flex flex-col gap-5">
      <p className="select-none text-2xl font-bold">Newest articles</p>
      <ArticleCarousel articles={articles} />
      {data.map((d) => {
        return (
          <EntryCarousel
            entries={d?.data.data.entries || []}
            title={d?.title}
          />
        )
      })}
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
