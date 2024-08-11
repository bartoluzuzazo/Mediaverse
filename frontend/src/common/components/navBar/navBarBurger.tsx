import {
  Dispatch,
  FunctionComponent,
  SetStateAction,
  useCallback,
  useEffect,
  useRef,
} from 'react'
import { AnimatePresence, motion } from 'framer-motion'
import { Link } from '@tanstack/react-router'

interface NavBarBurgerProps {
  isOpen: boolean
  setIsOpen: Dispatch<SetStateAction<boolean>>
}

const NavBarBurger: FunctionComponent<NavBarBurgerProps> = ({
  isOpen,
  setIsOpen,
}) => {
  const ref = useRef<HTMLDivElement>(null)

  const handleClickOutside = useCallback(
    (event: MouseEvent) => {
      if (
        (event.target as HTMLElement).tagName !== 'path' &&
        !(event.target as HTMLElement).classList.contains('burger-icon') &&
        isOpen &&
        ref.current &&
        !ref.current.contains(event.target as Node)
      ) {
        setIsOpen(false)
      }
    },
    [setIsOpen, isOpen, ref]
  )

  useEffect(() => {
    if (isOpen) document.addEventListener('click', handleClickOutside, false)
    return () => {
      document.removeEventListener('click', handleClickOutside, false)
    }
  }, [handleClickOutside, isOpen])

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          ref={ref}
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.2 }}
          exit={{ opacity: 0 }}
          className="border-mv-slate-200 shadow-mv-slate-200 absolute bottom-[-290px] origin-top rounded-md border border-solid bg-white p-5 shadow-md"
        >
          <ul className="flex flex-col gap-2">
            <li className="flex flex-col">
              <Link className="hover:text-mv-purple font-semibold text-black hover:underline">
                {' '}
                Quizzes
              </Link>
              <span className="text-mv-slate">
                Quizzes to check your movie knowledge
              </span>
            </li>
            <li className="flex flex-col">
              <Link className="hover:text-mv-purple font-semibold text-black hover:underline">
                Articles
              </Link>
              <span className="text-mv-slate">
                News from the world of culture
              </span>
            </li>
            <li className="flex flex-col">
              <Link className="hover:text-mv-purple font-semibold text-black hover:underline">
                Ask Me Anything
              </Link>
              <span className="text-mv-slate">Active AMA sessions</span>
            </li>
          </ul>
        </motion.div>
      )}
    </AnimatePresence>
  )
}

export default NavBarBurger
