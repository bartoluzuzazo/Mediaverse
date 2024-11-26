import { FunctionComponent, ReactNode } from 'react'

type Props = {
  isToggled: boolean
  toggle: () => void
  children: ReactNode
}

export const ToggleButton: FunctionComponent<Props> = ({
  isToggled,
  children,
  toggle,
}) => {
  return (
    <button
      className={`${isToggled ? 'bg-slate-900' : 'bg-slate-700'} mr-auto rounded-md px-3 py-1.5 text-white md:px-4 md:py-2`}
      onClick={toggle}
    >
      {children}
    </button>
  )
}
