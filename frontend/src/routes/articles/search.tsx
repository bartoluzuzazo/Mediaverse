import { createFileRoute } from '@tanstack/react-router'
import { ArticleSearch } from '../../common/components/articles/articleSearch'

export const Route = createFileRoute('/articles/search')({
  component: () => <ArticleSearch />,
})
