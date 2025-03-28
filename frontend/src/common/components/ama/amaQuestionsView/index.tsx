import { FunctionComponent, useEffect, useRef, useState } from 'react'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'
import {
  AmaQuestionOrder,
  AmaQuestionStatus,
  AmaStatus,
  GetAmaQuestionParams,
} from '../../../../models/amaSessions'
import { OrderDirection } from '../../../../models/common'
import { useInfiniteQuery } from '@tanstack/react-query'
import { amaSessionService } from '../../../../services/amaSessionService.ts'
import { useInView } from 'framer-motion'
import { AmaQuestionForm } from '../AmaQuestionForm'
import { AmaQuestionComponent } from '../amaQuestionComponent'
import { AmaQuestionSort } from './AmaQuestionSort.tsx'

type Props = {
  amaSessionId: string
  amaSessionStatus: AmaStatus
  managingUserId: string
  status: AmaQuestionStatus
}

export const AmaQuestionsView: FunctionComponent<Props> = ({
  amaSessionId,
  amaSessionStatus,
  managingUserId,
  status,
}) => {
  const { isAuthenticated } = useAuthContext()!
  const [questionsParams, setQuestionsParams] = useState<
    Omit<GetAmaQuestionParams, 'page' | 'status'>
  >({
    order: AmaQuestionOrder.TotalVotes,
    direction: OrderDirection.Descending,
    size: 20,
  })

  const queryKey = [
    'GET_AMA_QUESTIONS',
    amaSessionId,
    { ...questionsParams, status: status },
    { isAuthenticated },
  ]

  const { data, fetchNextPage } = useInfiniteQuery({
    queryKey: queryKey,
    queryFn: async ({ pageParam }) => {
      if (isAuthenticated) {
        const res = await amaSessionService.getAmaQuestionsAuthorized(
          amaSessionId,
          { ...questionsParams, page: pageParam, status: status }
        )
        return res.data
      } else {
        const res = await amaSessionService.getAmaQuestions(amaSessionId, {
          ...questionsParams,
          page: pageParam,
          status: status,
        })
        return res.data
      }
    },
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.nextPage,
  })
  const viewBoxRef = useRef(null)
  const isInView = useInView(viewBoxRef)
  useEffect(() => {
    if (isInView) {
      fetchNextPage()
    }
  }, [fetchNextPage, isInView])

  return (
    <div>
      {amaSessionStatus == AmaStatus.Active ? (
        <AmaQuestionForm
          amaSessionId={amaSessionId}
          parentQueryKeys={queryKey}
        />
      ) : null}
      <AmaQuestionSort setQuestionParams={setQuestionsParams} />
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
