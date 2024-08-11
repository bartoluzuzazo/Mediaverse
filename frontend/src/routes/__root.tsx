import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import NavBar from '../common/components/navBar'

export const Route = createRootRoute({
  component: () => (
    <>
      <NavBar />
      <div className="px-[150px]">
        <Outlet />
      </div>
      <TanStackRouterDevtools />
      <ReactQueryDevtools />
    </>
  ),
})
