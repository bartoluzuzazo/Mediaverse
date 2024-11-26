import { createFileRoute } from '@tanstack/react-router'
import MovieForm from '../../../../common/components/entries/movies/MovieForm.tsx'

export const Route = createFileRoute('/entries/movies/create/')({
  component: () => <MovieForm/>
})