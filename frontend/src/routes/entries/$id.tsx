import { createFileRoute } from '@tanstack/react-router'
import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { EntryService } from '../../services/entryService.ts'
import { BookEntryComponent } from '../../common/components/entries/books/BookEntryComponent.tsx'
import { MovieEntryComponent } from '../../common/components/entries/movies/MovieEntryComponent.tsx'

const entryTypeQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_ENTRY_TYPE', id],
    queryFn: async (): Promise<string> => {
      return await EntryService.getEntryType(id)
    },
  })
}

const EntryComponent = () => {
  const id = Route.useParams().id
  const typeQuery = useSuspenseQuery(entryTypeQueryOptions(id))
  const type = typeQuery.data
  const getComponent = () => {
    switch (type) {
      case 'Book': {
        return <BookEntryComponent id={id}/>
      }
      case 'Movie': {
        return <MovieEntryComponent id={id}/>
      }
    }
  }
  return getComponent()
}

export const Route = createFileRoute('/entries/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(entryTypeQueryOptions(id))
  },

  component: EntryComponent,
})