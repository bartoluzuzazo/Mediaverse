import { createFileRoute } from '@tanstack/react-router'
import { amaSessionService } from '../../services/amaSessionService.ts'
import { AmaQuestionStatus, AmaSession } from '../../models/amaSessions'
import { FunctionComponent } from 'react'
import { AmaSessionBanner } from '../../common/components/ama/amaSessionBanner'
import { AmaQuestionsView } from '../../common/components/ama/amaQuestionsView'
import { TabbedView } from '../../common/components/shared/TabbedView'
import { queryOptions } from '@tanstack/react-query'
import { dateUtil } from '../../utils/dateUtil.ts'

const amaSessionQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_AMA_SESSION', id],
    queryFn: async (): Promise<AmaSession> => {
      const response = await amaSessionService.getAmaSession(id)
      const data = response.data
      return {
        ...data,
        end: dateUtil.toHumanReadable(data.end),
        start: dateUtil.toHumanReadable(data.start),
      }
    },
  })
}

export const Route = createFileRoute('/ama-sessions/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(amaSessionQueryOptions(id))
  },
  component: () => <AmaSessionView />,
})

const AmaSessionView: FunctionComponent = () => {
  const amaSession = Route.useLoaderData<AmaSession>()
  return (
    <div className="-mx-[calc(50vw-50%)] min-h-screen bg-slate-100">
      <div className="mx-auto max-w-[70rem] p-2 pt-10">
        <AmaSessionBanner amaSession={amaSession} />

        <TabbedView
          tabs={[
            {
              key: 'All',
              element: (
                <AmaQuestionsView
                  status={AmaQuestionStatus.All}
                  amaSessionStatus={amaSession.status}
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                />
              ),
            },
            {
              key: 'Answered',
              element: (
                <AmaQuestionsView
                  status={AmaQuestionStatus.Answered}
                  amaSessionStatus={amaSession.status}
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                />
              ),
            },
            {
              key: 'Unanswered',
              element: (
                <AmaQuestionsView
                  status={AmaQuestionStatus.Unanswered}
                  amaSessionStatus={amaSession.status}
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                />
              ),
            },
          ]}
        />
      </div>
    </div>
  )
}
