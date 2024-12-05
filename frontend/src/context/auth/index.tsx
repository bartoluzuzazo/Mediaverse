import { ReactNode } from '@tanstack/react-router'
import axios from 'axios'
import {
  createContext,
  FunctionComponent,
  useEffect,
  useMemo,
  useState,
} from 'react'
import { useLocalStorage } from 'usehooks-ts'
import { jwtDecode } from 'jwt-decode'
import { Role } from '../../models/user'

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
  roles: string[]
}

interface JwtPayload {
  unique_name: string
  nameid: string
  email: string
  role: Role | Role[]
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

  const [tokenAxiosDefaults, authUserData] = useMemo(() => {
    if (!token) {
      axios.defaults.headers.common['Authorization'] = undefined
    }
    axios.defaults.headers.common['Authorization'] = token && `Bearer ${token}`

    const tokenFromAxios = axios.defaults.headers.common['Authorization']

    if (typeof tokenFromAxios !== 'string') {
      return [tokenFromAxios, undefined]
    }
    const jwtData = jwtDecode<JwtPayload>(tokenFromAxios)
    const roles = Array.isArray(jwtData.role) ? jwtData.role : [jwtData.role]
    const authUserData = {
      email: jwtData.email,
      id: jwtData.nameid,
      username: jwtData.unique_name,
      roles,
    }
    return [tokenFromAxios, authUserData]
  }, [token])

  useEffect(() => {
    axios.interceptors.response.use(
      (response) => {
        return response
      },
      (error) => {
        if (error.response.status === 401) {
          removeToken()
        }
        return error
      }
    )
    return () => {
      axios.interceptors.response.clear()
    }
  }, [axios])

  return (
    <AuthContext.Provider
      value={{
        token,
        setToken,
        removeToken,
        isAuthenticated: !!tokenAxiosDefaults,
        authUserData: authUserData,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export default AuthContextProvider
