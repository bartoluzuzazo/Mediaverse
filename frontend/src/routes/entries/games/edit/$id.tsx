import { createFileRoute } from '@tanstack/react-router'
import { Game } from '../../../../models/entry/game/Game.ts'
import { GameService } from '../../../../services/EntryServices/gameService.ts'
import GameForm from '../../../../common/components/entries/games/GameForm.tsx'

export const Route = createFileRoute('/entries/games/edit/$id')({
  loader: async ({ params }) => {
    const response = await GameService.getGame(params.id)
    return response.data
  },
  component: () => {
    const game = Route.useLoaderData<Game>()
    return game && <GameForm game={game} />
  },
})