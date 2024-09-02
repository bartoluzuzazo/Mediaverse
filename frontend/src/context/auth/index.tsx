import { ReactNode } from '@tanstack/react-router'
import axios from 'axios'
import { createContext, FunctionComponent, useEffect } from 'react'
import { useLocalStorage } from 'usehooks-ts'

export interface AuthContextProviderProps {
  children: ReactNode
}

export interface AuthContextProps {
  token?: string
  setToken: React.Dispatch<React.SetStateAction<string | undefined>>
  removeToken: () => void
  isAuthenticated: boolean
}

export const AuthContext = createContext<AuthContextProps | undefined>(
  undefined
)

const AuthContextProvider: FunctionComponent<AuthContextProviderProps> = ({
  children,
}) => {
  const [token, setToken, removeToken] = useLocalStorage<string | undefined>(
    'token',
    undefined
  )
  useEffect(() => {
    axios.defaults.headers.common['Authorization'] = token && `Bearer ${token}`
  }, [token])

  return (
    <AuthContext.Provider
      value={{ token, setToken, removeToken, isAuthenticated: !!token }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export default AuthContextProvider
