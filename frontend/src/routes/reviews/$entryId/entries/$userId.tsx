import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/reviews/$entryId/entries/$userId')({
  component: () => <div>Hello /reviews/$id/entries/$id!</div>
})