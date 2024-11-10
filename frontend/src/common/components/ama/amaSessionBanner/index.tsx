import { FunctionComponent } from 'react'
import { AmaSession } from '../../../../models/amaSessions'

type Props = {
  amaSession: AmaSession
}

export const AmaSessionBanner: FunctionComponent<Props> = ({ amaSession }) => {
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
      <div className="flex flex-col">
        <h1 className="mb-6 text-4xl font-bold md:text-6xl">
          {amaSession.title}
        </h1>
        <div className="flex-1 rounded-md border-[1px] border-slate-300 bg-white px-3 py-2 text-lg font-medium shadow-sm">
          {amaSession.description}
        </div>
        <div className="mt-2 flex items-center justify-between py-1">
          <div className="text-md font-bold text-slate-700">
            End of AMA session: {amaSession.end.replace('T', '  ')}
          </div>
          <button className="bg-violet-300 px-1.5 py-0.5 text-black">
            End Session
          </button>
        </div>
      </div>
    </div>
  )
}
