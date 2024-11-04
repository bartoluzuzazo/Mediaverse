import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/ama-sessions/$id')({
  component: () => <div>Hello /ama-sessions/$id!</div>
})