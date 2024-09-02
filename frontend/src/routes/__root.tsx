import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import NavBar from '../common/components/navBar'
import { useEffect } from 'react'
import { useLocalStorage } from 'usehooks-ts'
import axios from 'axios'
import { QueryClient } from '@tanstack/react-query'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()(
  {
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
          <div className="mx-auto max-w-[70rem]">
            <Outlet />
          </div>
          <TanStackRouterDevtools />
          <ReactQueryDevtools />
        </>
      )
    },
  }
)
