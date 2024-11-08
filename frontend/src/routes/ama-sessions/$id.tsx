import { createFileRoute } from '@tanstack/react-router'
import { amaSessionService } from '../../services/amaSessionService.ts'
import { AmaSession } from '../../models/amaSessions'
import { FunctionComponent } from 'react'
import { AmaSessionBanner } from '../../common/components/ama/amaSessionBanner'
import { AmaQuestionsView } from '../../common/components/ama/amaQuestionsView'
import { AmaQuestionForm } from '../../common/components/ama/AmaQuestionForm'

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
        <AmaQuestionsView amaSessionId={amaSession.id} />
      </div>
    </div>
  )
}
