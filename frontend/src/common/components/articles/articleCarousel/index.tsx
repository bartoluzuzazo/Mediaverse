import { FunctionComponent } from 'react'
import { CustomCarousel } from '../../shared/CustomCarousel'
import { Article } from '../../../../models/article'
import { Link } from '@tanstack/react-router'
import defaultImgUrl from '/person-icon.png'

interface ArticleCarouselProps {
  articles: Article[]
}

const responsive = {
  desktop: {
    breakpoint: {
      max: 3000,
      min: 1024,
    },
    items: 1,
    partialVisibilityGutter: 40,
  },
  mobile: {
    breakpoint: {
      max: 464,
      min: 0,
    },
    items: 1,
    partialVisibilityGutter: 30,
  },
  tablet: {
    breakpoint: {
      max: 1024,
      min: 464,
    },
    items: 1,
    partialVisibilityGutter: 30,
  },
}

const ArticleCarousel: FunctionComponent<ArticleCarouselProps> = ({
  articles,
}) => {
  return (
    <div>
      <CustomCarousel customResponsive={responsive}>
        {articles.map((article) => {
          const imgSrc =
            article.authorPicture &&
            'data:image/*;base64,' + article.authorPicture
          return (
            <Link
              key={article.id}
              className="m-1 my-3 block h-[235px] w-full overflow-hidden px-10 text-black transition-shadow visited:text-black hover:text-black hover:underline"
              to="/articles/$id"
              params={{ id: article.id }}
            >
              <div className="flex w-full flex-col justify-between">
                <div className="flex min-w-fit">
                  <img
                    src={imgSrc || defaultImgUrl}
                    className="mr-5 aspect-square h-16 rounded-full border-[1px] border-slate-200 bg-slate-300"
                    alt="cover photo"
                  />
                  <div className="flex flex-col justify-start">
                    <div className="flex flex-row font-bold">
                      {article.authorUsername}
                    </div>
                    <div className="flex flex-row font-bold">
                      {new Date(article.timestamp).toDateString()}
                    </div>
                  </div>
                </div>{' '}
                <div className="flex flex-col">
                  <p className="text-2xl font-semibold">{article.title}</p>
                  <p>{article.lede}</p>
                  <p
                    className="overflow-elipsis block"
                    dangerouslySetInnerHTML={{ __html: article.content }}
                  />
                </div>
              </div>
            </Link>
          )
        })}
      </CustomCarousel>
    </div>
  )
}

export default ArticleCarousel
