import { createFileRoute } from '@tanstack/react-router'
import { amaSessionService } from '../../services/amaSessionService.ts'
import { AmaQuestionStatus, AmaSession } from '../../models/amaSessions'
import { FunctionComponent } from 'react'
import { AmaSessionBanner } from '../../common/components/ama/amaSessionBanner'
import { AmaQuestionsView } from '../../common/components/ama/amaQuestionsView'
import { TabbedView } from '../../common/components/shared/TabbedView'

export const Route = createFileRoute('/ama-sessions/$id')({
  loader: async ({ params }) => {
    const response = await amaSessionService.getAmaSession(params.id)
    return response.data
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
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                  status={AmaQuestionStatus.All}
                />
              ),
            },
            {
              key: 'Answered',
              element: (
                <AmaQuestionsView
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                  status={AmaQuestionStatus.Answered}
                />
              ),
            },
            {
              key: 'Unanswered',
              element: (
                <AmaQuestionsView
                  amaSessionId={amaSession.id}
                  managingUserId={amaSession.authorUserId}
                  status={AmaQuestionStatus.Unanswered}
                />
              ),
            },
          ]}
        />
      </div>
    </div>
  )
}
