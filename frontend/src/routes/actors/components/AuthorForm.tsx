import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/actors/components/AuthorForm')({
  component: () => <div>Hello /actors/components/AuthorForm!</div>
})