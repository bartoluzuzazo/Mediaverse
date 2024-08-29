import { FunctionComponent, useRef, useState } from 'react'
import { AnimatePresence, motion } from 'framer-motion'
import LoginForm from './loginForm'
import RegisterForm from './registerForm'

interface AuthPanelProps {
  isOpen: boolean
}

const AuthPanel: FunctionComponent<AuthPanelProps> = ({ isOpen }) => {
  const ref = useRef<HTMLDivElement>(null)

  const [isLoginForm, setIsLoginForm] = useState(true)

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          ref={ref}
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.2 }}
          exit={{ opacity: 0 }}
          className="absolute right-[100px] top-[80px] z-40 rounded-md border border-solid border-mv-slate-200 bg-white p-5 shadow-md shadow-mv-slate-200"
        >
          {isLoginForm ? (
            <LoginForm setLoginPanelVisible={setIsLoginForm} />
          ) : (
            <RegisterForm setLoginPanelVisible={setIsLoginForm} />
          )}
        </motion.div>
      )}
    </AnimatePresence>
  )
}

export default AuthPanel
