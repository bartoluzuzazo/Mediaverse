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
          className="absolute bottom-[-290px] z-40 origin-top rounded-md border border-solid border-mv-slate-200 bg-white p-5 shadow-md shadow-mv-slate-200"
        >
          <ul className="flex flex-col gap-2">
            <li className="flex flex-col">
              <Link className="font-semibold text-black hover:text-mv-purple hover:underline">
                {' '}
                Quizzes
              </Link>
              <span className="text-mv-slate">
                Quizzes to check your movie knowledge
              </span>
            </li>
            <li className="flex flex-col">
              <Link className="font-semibold text-black hover:text-mv-purple hover:underline">
                Articles
              </Link>
              <span className="text-mv-slate">
                News from the world of culture
              </span>
            </li>
            <li className="flex flex-col">
              <Link className="font-semibold text-black hover:text-mv-purple hover:underline">
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
