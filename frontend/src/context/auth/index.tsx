import { ReactNode } from '@tanstack/react-router'
import axios from 'axios'
import { createContext, FunctionComponent, useEffect, useState } from 'react'
import { useLocalStorage } from 'usehooks-ts'
import { jwtDecode } from 'jwt-decode'

export interface AuthContextProviderProps {
  children: ReactNode
}

export interface AuthContextProps {
  token?: string
  setToken: React.Dispatch<React.SetStateAction<string | undefined>>
  removeToken: () => void
  isAuthenticated: boolean
  data: AuthData
}

export type AuthData =
  | { isAuthenticated: false }
  | {
      isAuthenticated: true
      username: string
      id: string
      email: string
    }

interface JwtPayload {
  unique_name: string
  nameid: string
  email: string
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
  const [data, setData] = useState<AuthData>({ isAuthenticated: false })
  useEffect(() => {
    axios.defaults.headers.common['Authorization'] = token && `Bearer ${token}`

    if (token) {
      const jwtData = jwtDecode<JwtPayload>(token)
      console.log(jwtData)
      setData({
        isAuthenticated: true,
        email: jwtData.email,
        id: jwtData.nameid,
        username: jwtData.unique_name,
      })
    } else {
      setData({ isAuthenticated: false })
    }
  }, [token])

  return (
    <AuthContext.Provider
      value={{
        token,
        setToken,
        removeToken,
        isAuthenticated: !!token,
        data,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export default AuthContextProvider
