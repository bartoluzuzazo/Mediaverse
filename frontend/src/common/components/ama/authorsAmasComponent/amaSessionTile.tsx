import { AmaSession } from '../../../../models/amaSessions'
import { FunctionComponent } from 'react'
import { Link } from '@tanstack/react-router'
import { AmaStatusIndicator } from './amaStatusIndicator.tsx'

interface AmaSessionTileProps {
  amaSession: AmaSession
}
export const AmaSessionTile: FunctionComponent<AmaSessionTileProps> = ({
  amaSession,
}) => {
  return (
    <Link
      to="/ama-sessions/$id"
      params={{ id: amaSession.id }}
      className="flex flex-col rounded-md border-[1px] border-slate-200 px-3 py-1.5 text-black shadow-md hover:text-slate-700 hover:shadow-lg md:flex-row"
    >
      <div className="text-bold flex-[2] basis-0 text-lg">
        {amaSession.title}
      </div>
      <div className="flex-[2] basis-0">
        <div>Start: {amaSession.start}</div>
        <div>End: {amaSession.end}</div>
      </div>
      <div className="grid flex-1 basis-0 place-content-center">
        <AmaStatusIndicator status={amaSession.status} />
      </div>
    </Link>
  )
}
