import { AmaStatus } from '../../../../models/amaSessions'
import { FunctionComponent } from 'react'

interface AmaStatusIndicatorProps {
  status: AmaStatus
}

export const AmaStatusIndicator: FunctionComponent<AmaStatusIndicatorProps> = ({
  status,
}) => {
  const text = mappings[status]
  return (
    <div className="rounded-md bg-violet-200 px-1.5 py-0.5 text-black hover:scale-105 hover:text-black">
      {text}
    </div>
  )
}
const mappings = {
  [AmaStatus.Upcoming]: 'Upcoming',
  [AmaStatus.Active]: 'Active',
  [AmaStatus.Cancelled]: 'Cancelled',
  [AmaStatus.Finished]: 'Finished',
}
