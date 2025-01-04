import { FunctionComponent, useEffect, useRef, useState } from 'react'
import { useDebounceValue } from 'usehooks-ts'
import { useInfiniteQuery } from '@tanstack/react-query'
import FormInput from '../../form/input'
import { FaSearch } from 'react-icons/fa'
import { articleService } from '../../../../services/articleService.ts'
import { useInView } from 'framer-motion'
import { ArticlePreviewComponent } from './articlePreviewComponent.tsx'

interface ArticleSearchProps {}

export const ArticleSearch: FunctionComponent<ArticleSearchProps> = () => {
  const [query, setQuery] = useState('')
  const [debouncedQuery] = useDebounceValue(query, 300)
  const {
    data: articlePages,
    fetchNextPage,
    // isFetchingNextPage,
  } = useInfiniteQuery({
    queryKey: ['SEARCH_ARTICLES', { query: debouncedQuery }],
    queryFn: async ({ pageParam }) => {
      return (
        await articleService.searchArticles({
          searched: debouncedQuery,
          page: pageParam,
          size: 10,
        })
      ).data
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
    enabled: !!debouncedQuery,
  })

  const viewBoxRef = useRef(null)
  const isInView = useInView(viewBoxRef)

  useEffect(() => {
    if (isInView && !!debouncedQuery) {
      fetchNextPage()
    }
  }, [fetchNextPage, isInView])

  return (
    <div className="mt-6">
      <FormInput
        inputProps={{
          type: 'text',
          placeholder: 'Search articles...',
          value: query,
          onChange: (e) => setQuery(e.target.value),
        }}
        rightElement={<FaSearch />}
      />
      <div ref={viewBoxRef} className="min-h-1" />
      <div>
        {articlePages?.pages
          .flatMap((page) => page.contents)
          .map((article) => (
            <ArticlePreviewComponent article={article} key={article.id} />
          ))}
      </div>
    </div>
  )
}
