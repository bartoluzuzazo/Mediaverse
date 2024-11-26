import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'
import { MovieService } from '../../../services/movieService.ts'
import EntryAuthorPreview from '../../../common/components/entries/entryAuthorPreview.tsx'
import SectionHeader from '../../../common/components/entries/sectionHeader.tsx'
import CommentSection from '../../../common/components/comments/CommentSection.tsx'
import { AuthorizedView } from '../../../common/components/auth/AuthorizedView'
import { LinkButton } from '../../../common/components/shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Movie } from '../../../models/entry/movie/Movie.ts'

interface MovieEntryComponentProps {}

const movieQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_MOVIE', id],
    queryFn: async (): Promise<Movie> => {
      const res = await MovieService.getMovie(id)
      return res.data
    },
  })
}

const MovieEntryComponent: FunctionComponent<MovieEntryComponentProps> = () => {
  const id = Route.useParams().id
  const movieQuery = useSuspenseQuery(movieQueryOptions(id))
  const movie = movieQuery.data
  const info = [movie.entry.release.toString(), ...movie.cinematicGenres]

  return (
    <>
      <EntryBanner entry={movie.entry} info={info} type={'Movie'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/movies/edit/$id'} params={{id: movie.entry.id}} icon={<FaPen/>}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={movie.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{movie.entry.description}</div>
      {movie.entry.authors.map((group) => (
        <Fragment key={group.role}>
          <SectionHeader title={group.role} />
          <div className="flex flex-wrap">
            {group.authors.map((a) => (
              <div className="p-2" key={a.id}>
                <EntryAuthorPreview author={a} />
              </div>
            ))}
          </div>
        </Fragment>
      ))}
      <SectionHeader title={'Synopsis'} />
      <div className="p-4">{movie.synopsis}</div>
      <CommentSection entryId={id} />
    </>
  )
}

export const Route = createFileRoute('/entries/movies/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(movieQueryOptions(id))
  },

  component: MovieEntryComponent,
})
