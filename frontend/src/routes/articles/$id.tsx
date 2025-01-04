import { createFileRoute } from '@tanstack/react-router'
import { FunctionComponent } from 'react'
import { articleService } from '../../services/articleService.ts'
import MarkdownPreview from '@uiw/react-markdown-preview'
import defaultImgUrl from '/person-icon.png'
import { AuthorizedView } from '../../common/components/auth/AuthorizedView'
import { LinkButton } from '../../common/components/shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { useAuthContext } from '../../context/auth/useAuthContext.ts'

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
    <article className="mt-6">
      <h1>{article.title}</h1>
      <div className="flex justify-between">
        <div>
          <p className="text-lg italic text-slate-800">{article.lede}</p>
          <div>
            <AuthorizedView
              allowedRoles="ContentCreator"
              requiredUserId={article.userId}
            >
              <div className="my-2 max-w-fit">
                <LinkButton
                  icon={<FaPen />}
                  to="/articles/edit/$id"
                  params={{ id: article.id }}
                >
                  Edit article
                </LinkButton>
              </div>
            </AuthorizedView>
          </div>
        </div>
        <div className="min-w-fit">
          <img
            src={imgSrc || defaultImgUrl}
            className="aspect-square h-24 rounded-full border-[1px] border-slate-200 bg-slate-300"
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
