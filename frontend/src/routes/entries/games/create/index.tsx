import { createFileRoute } from '@tanstack/react-router'
import GameForm from '../../../../common/components/entries/games/GameForm.tsx'

export const Route = createFileRoute('/entries/games/create/')({
  component: () => <GameForm/>
})