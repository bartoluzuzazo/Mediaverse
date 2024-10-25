import {
  Dispatch,
  FunctionComponent,
  SetStateAction,
  useCallback,
  useEffect,
  useRef,
} from 'react'
import { AnimatePresence, motion } from 'framer-motion'
import { NavLinks } from './NavLinks.tsx'

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
          className="absolute top-[80px] z-40 origin-top rounded-md border border-solid border-mv-slate-200 bg-white p-5 shadow-md shadow-mv-slate-200"
        >
          <NavLinks/>

        </motion.div>
      )}
    </AnimatePresence>
  )
}

export default NavBarBurger
