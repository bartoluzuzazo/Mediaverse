import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import NavBar from '../common/components/navBar'
import { QueryClient } from '@tanstack/react-query'
import AuthContextProvider from '../context/auth'
import SearchPanelContext from '../context/searchPanel'
import SearchPanel from '../common/components/searchPanel'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()(
  {
    component: () => {
      return (
        <AuthContextProvider>
          <SearchPanelContext>
            <NavBar />
            <div className="relative mx-auto max-w-[70rem]">
              <SearchPanel />
              <Outlet />
            </div>
          </SearchPanelContext>
        </AuthContextProvider>
      )
    },
  }
)
