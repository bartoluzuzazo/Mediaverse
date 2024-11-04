import { FunctionComponent, ReactNode, useState } from 'react'
import { FaLink } from 'react-icons/fa'
import { Modal } from '../Modal'

interface Props {
  children: ReactNode
}

export const ModalButton: FunctionComponent<Props> = ({children}) => {
  const [isOpen, setIsOpen] = useState<boolean>(false)
  return (
    <>
      <button
        className="mt-2 flex w-full items-center gap-3 rounded-md border-none bg-violet-200 p-1 hover:scale-105"
        onClick={() => setIsOpen(true)}
      >
        <div className="m-1 grid aspect-square w-10 place-content-center self-center rounded-full bg-violet-900 text-white">
          <FaLink className="text-xl" />
        </div>
        <span className="text-lg font-semibold">Link Author</span>
      </button>
      {isOpen && (
        <Modal onOutsideClick={() => setIsOpen(false)}>
					{children}
        </Modal>
      )}
    </>
  )
}
