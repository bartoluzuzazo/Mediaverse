import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import NavBar from '../common/components/navBar'
import { QueryClient } from '@tanstack/react-query'
import AuthContextProvider from '../context/auth'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()(
  {
    component: () => {
      return (
        <AuthContextProvider>
          <NavBar />
          <div className="mx-auto max-w-[70rem]">
            <Outlet />
          </div>
        </AuthContextProvider>
      )
    },
  }
)
