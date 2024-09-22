import { FunctionComponent, MouseEventHandler, ReactNode } from 'react'

type Props = {
  icon: ReactNode
  onClick: MouseEventHandler<HTMLButtonElement>
  text: string
  isSecondary?: boolean
}
export const FriendshipButtonLayout: FunctionComponent<Props> = ({
  onClick,
  icon,
  text,
  isSecondary,
}) => {
  return (
    <button
      onClick={onClick}
      type="button"
      className={`mt-2 flex w-full items-center gap-3 px-4 py-2 ${
        isSecondary ? `bg-slate-300 text-slate-950` : `bg-slate-950 text-white`
      }`}
    >
      {icon}
      <span>{text}</span>
    </button>
  )
}
