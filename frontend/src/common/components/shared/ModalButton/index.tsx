import { FunctionComponent, ReactNode, useState } from 'react'
import { Modal } from '../Modal'

interface Props {
  children: ReactNode
  icon: ReactNode
  text: string
}

export const ModalButton: FunctionComponent<Props> = ({children, text, icon}) => {
  const [isOpen, setIsOpen] = useState<boolean>(false)
  return (
    <>
      <button
        className="mt-2 flex w-full items-center gap-3 rounded-md border-none bg-violet-200 p-1 hover:scale-105"
        onClick={() => setIsOpen(true)}
      >
        <div className="m-1 grid aspect-square w-10 place-content-center self-center rounded-full bg-violet-900 text-white">
          {icon}
        </div>
        <span className="text-lg font-semibold">{text}</span>
      </button>
      {isOpen && (
        <Modal onOutsideClick={() => setIsOpen(false)}>
					{children}
        </Modal>
      )}
    </>
  )
}
