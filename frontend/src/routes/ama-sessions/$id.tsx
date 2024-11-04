import { createFileRoute } from '@tanstack/react-router'
import { amaSessionService } from '../../services/amaSessionService.ts'
import { AmaSession } from '../../models/amaSessions'
import { FunctionComponent } from 'react'
import { AmaSessionBanner } from '../../common/components/ama/amaSessionBanner'

export const Route = createFileRoute('/ama-sessions/$id')({
  loader: async ({ params }) => {
    const response = await amaSessionService.getAmaSession(params.id)
    return response.data
  },
  component: () => <AmaSessionView/>,
})


const AmaSessionView: FunctionComponent = () => {
  const amaSession = Route.useLoaderData<AmaSession>()
  return (
    <div className='min-h-screen -mx-[calc(50vw-50%)] bg-slate-100'>
      <div className="mx-auto max-w-[70rem] pt-10 p-2">
        <AmaSessionBanner amaSession={amaSession}/>
      </div>
    </div>
  )
}
