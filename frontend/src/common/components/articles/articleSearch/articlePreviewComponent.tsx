import { FunctionComponent } from 'react'
import { ArticlePreview } from '../../../../models/article'
import defaultImgUrl from '/person-icon.png'
import { Link } from '@tanstack/react-router'

type Props = {
  article: ArticlePreview
}

export const ArticlePreviewComponent: FunctionComponent<Props> = ({
  article,
}) => {
  const imgSrc =
    article.authorPicture && 'data:image/*;base64,' + article.authorPicture
  return (
    <Link
      to="/articles/$id"
      params={{ id: article.id }}
      className="group my-2 block rounded-md border border-solid border-slate-200 p-4 text-black shadow-lg hover:text-black"
    >
      <div className="flex justify-between">
        <div>
          <h2 className="text-3xl font-bold group-hover:underline">
            {article.title}
          </h2>
          <p className="text-lg italic text-slate-800">{article.lede}</p>
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
    </Link>
  )
}
