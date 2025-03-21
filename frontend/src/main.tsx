import React from 'react'
import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { routeTree } from './routeTree.gen'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import axios from 'axios'

import './index.css'
import 'react-multi-carousel/lib/styles.css'
import 'react-loading-skeleton/dist/skeleton.css'
import 'react-tabs/style/react-tabs.css'

const queryClient = new QueryClient()

const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
})

axios.defaults.baseURL = import.meta.env.PROD
  ? '/api/'
  : import.meta.env.VITE_API_ADDRESS || '/api/'

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </React.StrictMode>
)
