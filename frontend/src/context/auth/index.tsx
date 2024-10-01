import { ReactNode } from '@tanstack/react-router'
import axios from 'axios'
import { createContext, FunctionComponent, useEffect, useMemo } from 'react'
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
  authUserData?: AuthData
}

export interface AuthData {
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
  undefined,
)

const AuthContextProvider: FunctionComponent<AuthContextProviderProps> = ({
                                                                            children,
                                                                          }) => {
  const [token, setToken, removeToken] = useLocalStorage<string | undefined>(
    'token',
    undefined,
  )

  useEffect(() => {
    axios.defaults.headers.common['Authorization'] = token && `Bearer ${token}`
  }, [token])

  const authUserData: AuthData | undefined = useMemo(() => {
    if (!token) {
      return undefined
    }
    const jwtData = jwtDecode<JwtPayload>(token)
    return {
      email: jwtData.email,
      id: jwtData.nameid,
      username: jwtData.unique_name,
    }
  }, [token])

  return (
    <AuthContext.Provider
      value={{
        token,
        setToken,
        removeToken,
        isAuthenticated: !!token,
        authUserData: authUserData,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export default AuthContextProvider
