import { createFileRoute } from '@tanstack/react-router'
import MarkdownEditor from '@uiw/react-markdown-editor'

export const Route = createFileRoute('/articles/create')({
  component: () => <CreateArticle />,
})
const CreateArticle = () => {
  return (
    <div>
      <h1>Hello</h1>
      <MarkdownEditor />
    </div>
  )
}
