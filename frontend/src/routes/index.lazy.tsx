import { createLazyFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import EntryCarousel from '../common/components/entries/entryCarousel'
import { useQueries } from '@tanstack/react-query'
import { BookService } from '../services/bookService'
import { EntryOrder } from '../models/entry/Entry'
import { OrderDirection } from '../models/common'

interface DashboardProps {}

const Dashboard: FunctionComponent<DashboardProps> = () => {
  const [topBooks, newestBooks] = useQueries({
    queries: [
      {
        queryKey: ['top', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            entryOrder: EntryOrder.Rating,
            direction: OrderDirection.Descending,
          }),
      },
      {
        queryKey: ['newest', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            entryOrder: EntryOrder.Release,
            direction: OrderDirection.Descending,
          }),
      },
    ],
  })

  return (
    <div>
      <EntryCarousel books={topBooks.data?.data.books || []} />
      <EntryCarousel books={newestBooks.data?.data.books || []} />
    </div>
  )
}

export const Route = createLazyFileRoute('/')({
  component: Dashboard,
})

export default Dashboard
