import { FunctionComponent, ReactNode } from 'react'
import { createPortal } from 'react-dom'

type Props = {
  children: ReactNode
  onOutsideClick: () => void | Promise<void>
}

export const Modal: FunctionComponent<Props> = ({ children, onOutsideClick }) => {
  return createPortal(
    <>
      <div className='fixed inset-0 backdrop-blur-md z-40' onClick={onOutsideClick}></div>
      <div className={`max-w-[800px] h-[600px] overflow-y-scroll w-full p-4  rounded-md bg-white shadow-md fixed top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 z-50`}>
        {children}
      </div>
    </>,
    document.getElementById('modal')!)
}