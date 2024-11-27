import { createFileRoute } from '@tanstack/react-router'
import { BookService } from '../services/bookService'
import { EntryOrder } from '../models/entry/Entry'
import { OrderDirection } from '../models/common'
import { Oval } from 'react-loader-spinner'
import { MovieService } from '../services/movieService.ts'

export const Route = createFileRoute('/')({
  pendingComponent: () => (
    <div className="pointer-events-none fixed bottom-0 left-0 right-0 top-0 flex items-center justify-center">
      <Oval
        visible={true}
        height="80"
        width="80"
        color="#5b21b6"
        secondaryColor=""
        ariaLabel="oval-loading"
        wrapperStyle={{}}
        wrapperClass=""
      />
    </div>
  ),
  loader: async ({ context: { queryClient } }) => {
    return [
      await queryClient.ensureQueryData({
        queryKey: ['top', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            order: EntryOrder.Rating,
            direction: OrderDirection.Descending,
          }),
      }),
      await queryClient.ensureQueryData({
        queryKey: ['newest', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            order: EntryOrder.Release,
            direction: OrderDirection.Descending,
          }),
      }),

      await queryClient.ensureQueryData({
        queryKey: ['top', 'movies'],
        queryFn: async () =>
          await MovieService.getMovies({
            page: 1,
            size: 10,
            order: EntryOrder.Rating,
            direction: OrderDirection.Descending,
          }),
      }),
      await queryClient.ensureQueryData({
        queryKey: ['newest', 'movies'],
        queryFn: async () =>
          await MovieService.getMovies({
            page: 1,
            size: 10,
            order: EntryOrder.Release,
            direction: OrderDirection.Descending,
          }),
      }),
    ]
  },
})
