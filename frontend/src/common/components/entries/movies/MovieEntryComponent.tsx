import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { MovieService } from '../../../../services/movieService.ts'
import EntryAuthorPreview from '../entryAuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Movie } from '../../../../models/entry/movie/Movie.ts'
import { ReviewsCarousel } from '../../reviews/reviewsCarousel'

interface MovieEntryComponentProps {
  id: string
}

const movieQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_MOVIE', id],
    queryFn: async (): Promise<Movie> => {
      const res = await MovieService.getMovie(id)
      return res.data
    },
  })
}

export const MovieEntryComponent: FunctionComponent<
  MovieEntryComponentProps
> = ({ id }) => {
  const movieQuery = useSuspenseQuery(movieQueryOptions(id))
  const movie = movieQuery.data
  const info = [movie.entry.release.toString(), ...movie.cinematicGenres]

  return (
    <>
      <EntryBanner entry={movie.entry} info={info} type={'Movie'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className="-mb-2 mt-4 max-w-32">
          <LinkButton
            to={'/entries/movies/edit/$id'}
            params={{ id: movie.entry.id }}
            icon={<FaPen />}
          >
            Edit
          </LinkButton>
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
      <ReviewsCarousel entryId={id} />
      <CommentSection entryId={id} />
    </>
  )
}
