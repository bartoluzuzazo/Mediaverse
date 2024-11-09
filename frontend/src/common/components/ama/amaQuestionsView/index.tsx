import { FunctionComponent, useEffect, useRef, useState } from 'react'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import {
  AmaQuestionOrder,
  AmaQuestionStatus,
  GetAmaQuestionParams,
} from '../../../../models/amaSessions'
import { OrderDirection } from '../../../../models/common'
import { useInfiniteQuery } from '@tanstack/react-query'
import { amaSessionService } from '../../../../services/amaSessionService.ts'
import { useInView } from 'framer-motion'
import { AmaQuestionForm } from '../AmaQuestionForm'
import { AmaQuestionComponent } from '../amaQuestionComponent'

type Props = {
  amaSessionId: string
  managingUserId: string
}

export const AmaQuestionsView: FunctionComponent<Props> = ({
  amaSessionId,
  managingUserId,
}) => {
  const { isAuthenticated } = useAuthContext()!
  const [questionsParams] = useState<Omit<GetAmaQuestionParams, 'page'>>({
    order: AmaQuestionOrder.TotalVotes,
    direction: OrderDirection.Descending,
    size: 20,
    status: AmaQuestionStatus.All,
  })
  const queryKey = [
    'GET_AMA_QUESTIONS',
    amaSessionId,
    { questionsParams },
    { isAuthenticated },
  ]

  const { data, fetchNextPage } = useInfiniteQuery({
    queryKey: queryKey,
    queryFn: async ({ pageParam }) => {
      if (isAuthenticated) {
        const res = await amaSessionService.getAmaQuestionsAuthorized(
          amaSessionId,
          { ...questionsParams, page: pageParam }
        )
        return res.data
      } else {
        const res = await amaSessionService.getAmaQuestions(amaSessionId, {
          ...questionsParams,
          page: pageParam,
        })
        return res.data
      }
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
  })
  // TODO: refactor maybe, or maybe not, idk
  const viewBoxRef = useRef(null)
  const isInView = useInView(viewBoxRef)
  useEffect(() => {
    if (isInView) {
      fetchNextPage()
    }
  }, [fetchNextPage, isInView])

  return (
    <div>
      <AmaQuestionForm amaSessionId={amaSessionId} parentQueryKeys={queryKey} />
      {data &&
        data.pages.map((page) =>
          page.contents.map((q) => {
            return (
              <AmaQuestionComponent
                page={page.currentPage}
                question={q}
                managingUserId={managingUserId}
                key={q.id}
                parentQueryKey={queryKey}
              />
            )
          })
        )}
    </div>
  )
}
