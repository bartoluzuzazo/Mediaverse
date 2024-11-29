import { FunctionComponent } from 'react'
import { AmaSession, AmaStatus } from '../../../../models/amaSessions'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { EndSessionComponent } from './endSessionComponent.tsx'

interface AmaSessionBannerProps {
  amaSession: AmaSession
}

export const AmaSessionBanner: FunctionComponent<AmaSessionBannerProps> = ({
  amaSession,
}) => {
  return (
    <div className="my-3 md:flex md:items-stretch md:gap-6">
      <div className="flex justify-center">
        <div>
          <img
            src={`data:image/webp;base64,${amaSession.profilePicture}`}
            className="aspect-square w-52 rounded-full border-[6px] border-violet-700 bg-slate-300 object-cover"
            alt="profile picture"
          />
          <div className="text-xl font-bold md:text-center">
            {amaSession.authorName}{' '}
            <span className="font-italic">{amaSession.authorSurname}</span>
          </div>
        </div>
      </div>
      <div className="flex flex-1 flex-col">
        <h1 className="mb-6 text-4xl font-bold md:text-6xl">
          {amaSession.title}
        </h1>
        <div className="flex-1 rounded-md border-[1px] border-slate-300 bg-white px-3 py-2 text-lg font-medium shadow-sm">
          {amaSession.description}
        </div>
        <div className="mt-2 flex items-center justify-between py-1">
          <AmaSessionTimeComponent amaSession={amaSession} />
          <AuthorizedView requiredUserId={amaSession.authorUserId}>
            <EndSessionComponent
              amaSessionId={amaSession.id}
              amaSessionStatus={amaSession.status}
              authorId={amaSession.authorId}
            />
          </AuthorizedView>
        </div>
      </div>
    </div>
  )
}

interface AmaSessionTimeComponentProps {
  amaSession: AmaSession
}

const AmaSessionTimeComponent: FunctionComponent<
  AmaSessionTimeComponentProps
> = ({ amaSession }) => {
  const status = amaSession.status
  const start = amaSession.start
  const end = amaSession.end
  const content =
    status === AmaStatus.Upcoming
      ? `Start of AMA session: ${start}`
      : status === AmaStatus.Cancelled
        ? `Session cancelled at: ${end}`
        : status === AmaStatus.Active
          ? `End of AMA session: ${end}`
          : `AMA session ended at: ${end}`

  return <div className="text-md font-bold text-slate-700">{content}</div>
}
