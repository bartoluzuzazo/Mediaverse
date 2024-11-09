import { createFileRoute } from '@tanstack/react-router'
import { Movie } from '../../../../models/entry/movie/Movie.ts'
import MovieForm from '../../../../common/components/entries/movies/MovieForm.tsx'
import { MovieService } from '../../../../services/movieService.ts'

export const Route = createFileRoute('/entries/movies/edit/$id')({
  loader: async ({ params }) => {
    const response = await MovieService.getMovie(params.id)
    return response.data
  },
  component: () => {
    const movie = Route.useLoaderData<Movie>()
    return movie && <MovieForm movie={movie} />
  },
})