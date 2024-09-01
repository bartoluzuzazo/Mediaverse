import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import NavBar from '../common/components/navBar'
import { useEffect } from 'react'
import { useLocalStorage } from 'usehooks-ts'
import axios from 'axios'

export const Route = createRootRoute({
  component: () => {
    const [token, _] = useLocalStorage<string | undefined>('token', undefined)
    useEffect(() => {
      if (token) {
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
      }
    }, [])
    return (
      <>
        <NavBar />
        <div className="mx-auto max-w-[60rem]">
          <Outlet />
        </div>
        <TanStackRouterDevtools />
        <ReactQueryDevtools />
      </>
    )
  },
})
