import { createFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import { articleService } from '../../services/articleService.ts'
import MarkdownPreview from '@uiw/react-markdown-preview'
import defaultImgUrl from '/person-icon.png'

export const Route = createFileRoute('/articles/$id')({
  loader: async ({ params }) => {
    const response = await articleService.getArticle(params.id)
    return response.data
  },
  component: () => <ArticleComponent />,
})

export const ArticleComponent: FunctionComponent = () => {
  const article = Route.useLoaderData()
  const imgSrc =
    article.authorPicture && 'data:image/*;base64,' + article.authorPicture
  return (
    <article>
      <h1>{article.title}</h1>
      <div className="flex justify-between">
        <p className="text-lg italic text-slate-800">{article.lede}</p>
        <div>
          <img
            src={imgSrc || defaultImgUrl}
            className="aspect-square h-24 rounded-full bg-slate-300 p-1"
            alt="cover photo"
          />
          <div className="flex flex-row justify-center font-bold">
            {article.authorUsername}
          </div>
        </div>
      </div>
      <MarkdownPreview
        source={article.content}
        wrapperElement={{ 'data-color-mode': 'light' }}
      />
    </article>
  )
}
