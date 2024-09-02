import { createFileRoute } from '@tanstack/react-router'
import { BookService } from '../services/bookService'
import { EntryOrder } from '../models/entry/Entry'
import { OrderDirection } from '../models/common'

export const Route = createFileRoute('/')({
  loader: async ({ context: { queryClient } }) => {
    return [
      await queryClient.ensureQueryData({
        queryKey: ['top', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            entryOrder: EntryOrder.Rating,
            direction: OrderDirection.Descending,
          }),
      }),
      await queryClient.ensureQueryData({
        queryKey: ['newest', 'books'],
        queryFn: async () =>
          await BookService.getBooks({
            page: 1,
            size: 10,
            entryOrder: EntryOrder.Release,
            direction: OrderDirection.Descending,
          }),
      }),
    ]
  },
})
