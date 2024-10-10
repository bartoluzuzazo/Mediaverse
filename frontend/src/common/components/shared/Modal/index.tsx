import { FunctionComponent, ReactNode } from 'react'
import { createPortal } from 'react-dom'

type Props = {
  children: ReactNode
  onOutsideClick: () => void | Promise<void>
}

export const Modal: FunctionComponent<Props> = ({
  children,
  onOutsideClick,
}) => {
  return createPortal(
    <>
      <div
        className="fixed inset-0 z-40 backdrop-blur-md"
        onClick={onOutsideClick}
      ></div>
      <div
        className={`fixed left-1/2 top-1/2 z-50 h-[600px] w-full max-w-[800px] -translate-x-1/2 -translate-y-1/2 rounded-md bg-white p-4 shadow-md`}
      >
        {children}
      </div>
    </>,
    document.getElementById('modal')!
  )
}
