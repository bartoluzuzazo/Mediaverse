import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { GameService } from '../../../../services/EntryServices/gameService.ts'
import EntryAuthorPreview from '../AuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { Game } from '../../../../models/entry/game/Game.ts'
import { ReviewsCarousel } from '../../reviews/reviewsCarousel'

interface GameEntryComponentProps {
  id: string
}

const gameQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_ENTRY', id],
    queryFn: async (): Promise<Game> => {
      const res = await GameService.getGame(id)
      return res.data
    },
  })
}

export const GameEntryComponent: FunctionComponent<GameEntryComponentProps> = ({
  id,
}) => {
  const gameQuery = useSuspenseQuery(gameQueryOptions(id))
  const game = gameQuery.data
  const info = [game.entry.release.toString(), ...game.gameGenres]

  return (
    <>
      <EntryBanner entry={game.entry} info={info} type={'Game'} />
      <AuthorizedView allowedRoles="ContentCreator">
        <div className="-mb-2 mt-4 max-w-32">
          <LinkButton
            to={'/entries/games/edit/$id'}
            params={{ id: game.entry.id }}
            icon={<FaPen />}
          >
            Edit
          </LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={game.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{game.entry.description}</div>
      {game.entry.authors.map((group) => (
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
      <div className="p-4">{game.synopsis}</div>
      <ReviewsCarousel entryId={id} />
      <CommentSection entryId={id} />
    </>
  )
}
