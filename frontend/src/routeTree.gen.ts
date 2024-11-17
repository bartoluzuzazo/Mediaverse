/* prettier-ignore-start */

/* eslint-disable */

// @ts-nocheck

// noinspection JSUnusedGlobalSymbols

// This file is auto-generated by TanStack Router

// Import Routes

import { Route as rootRoute } from './routes/__root'
import { Route as IndexImport } from './routes/index'
import { Route as SearchIndexImport } from './routes/search/index'
import { Route as UsersSearchImport } from './routes/users/search'
import { Route as UsersIdImport } from './routes/users/$id'
import { Route as AuthorsIdImport } from './routes/authors/$id'
import { Route as AuthorsCreateIndexImport } from './routes/authors/create/index'
import { Route as UsersEditIdImport } from './routes/users/edit/$id'
import { Route as EntriesBooksIdImport } from './routes/entries/books/$id'
import { Route as AuthorsEditIdImport } from './routes/authors/edit/$id'
import { Route as EntriesBooksCreateIndexImport } from './routes/entries/books/create/index'
import { Route as EntriesBooksEditIdImport } from './routes/entries/books/edit/$id'

// Create/Update Routes

const IndexRoute = IndexImport.update({
  path: '/',
  getParentRoute: () => rootRoute,
} as any).lazy(() => import('./routes/index.lazy').then((d) => d.Route))

const SearchIndexRoute = SearchIndexImport.update({
  path: '/search/',
  getParentRoute: () => rootRoute,
} as any)

const UsersSearchRoute = UsersSearchImport.update({
  path: '/users/search',
  getParentRoute: () => rootRoute,
} as any)

const UsersIdRoute = UsersIdImport.update({
  path: '/users/$id',
  getParentRoute: () => rootRoute,
} as any)

const AuthorsIdRoute = AuthorsIdImport.update({
  path: '/authors/$id',
  getParentRoute: () => rootRoute,
} as any)

const AuthorsCreateIndexRoute = AuthorsCreateIndexImport.update({
  path: '/authors/create/',
  getParentRoute: () => rootRoute,
} as any)

const UsersEditIdRoute = UsersEditIdImport.update({
  path: '/users/edit/$id',
  getParentRoute: () => rootRoute,
} as any)

const EntriesBooksIdRoute = EntriesBooksIdImport.update({
  path: '/entries/books/$id',
  getParentRoute: () => rootRoute,
} as any)

const AuthorsEditIdRoute = AuthorsEditIdImport.update({
  path: '/authors/edit/$id',
  getParentRoute: () => rootRoute,
} as any)

const EntriesBooksCreateIndexRoute = EntriesBooksCreateIndexImport.update({
  path: '/entries/books/create/',
  getParentRoute: () => rootRoute,
} as any)

const EntriesBooksEditIdRoute = EntriesBooksEditIdImport.update({
  path: '/entries/books/edit/$id',
  getParentRoute: () => rootRoute,
} as any)

// Populate the FileRoutesByPath interface

declare module '@tanstack/react-router' {
  interface FileRoutesByPath {
    '/': {
      id: '/'
      path: '/'
      fullPath: '/'
      preLoaderRoute: typeof IndexImport
      parentRoute: typeof rootRoute
    }
    '/authors/$id': {
      id: '/authors/$id'
      path: '/authors/$id'
      fullPath: '/authors/$id'
      preLoaderRoute: typeof AuthorsIdImport
      parentRoute: typeof rootRoute
    }
    '/users/$id': {
      id: '/users/$id'
      path: '/users/$id'
      fullPath: '/users/$id'
      preLoaderRoute: typeof UsersIdImport
      parentRoute: typeof rootRoute
    }
    '/users/search': {
      id: '/users/search'
      path: '/users/search'
      fullPath: '/users/search'
      preLoaderRoute: typeof UsersSearchImport
      parentRoute: typeof rootRoute
    }
    '/search/': {
      id: '/search/'
      path: '/search'
      fullPath: '/search'
      preLoaderRoute: typeof SearchIndexImport
      parentRoute: typeof rootRoute
    }
    '/authors/edit/$id': {
      id: '/authors/edit/$id'
      path: '/authors/edit/$id'
      fullPath: '/authors/edit/$id'
      preLoaderRoute: typeof AuthorsEditIdImport
      parentRoute: typeof rootRoute
    }
    '/entries/books/$id': {
      id: '/entries/books/$id'
      path: '/entries/books/$id'
      fullPath: '/entries/books/$id'
      preLoaderRoute: typeof EntriesBooksIdImport
      parentRoute: typeof rootRoute
    }
    '/users/edit/$id': {
      id: '/users/edit/$id'
      path: '/users/edit/$id'
      fullPath: '/users/edit/$id'
      preLoaderRoute: typeof UsersEditIdImport
      parentRoute: typeof rootRoute
    }
    '/authors/create/': {
      id: '/authors/create/'
      path: '/authors/create'
      fullPath: '/authors/create'
      preLoaderRoute: typeof AuthorsCreateIndexImport
      parentRoute: typeof rootRoute
    }
    '/entries/books/edit/$id': {
      id: '/entries/books/edit/$id'
      path: '/entries/books/edit/$id'
      fullPath: '/entries/books/edit/$id'
      preLoaderRoute: typeof EntriesBooksEditIdImport
      parentRoute: typeof rootRoute
    }
    '/entries/books/create/': {
      id: '/entries/books/create/'
      path: '/entries/books/create'
      fullPath: '/entries/books/create'
      preLoaderRoute: typeof EntriesBooksCreateIndexImport
      parentRoute: typeof rootRoute
    }
  }
}

// Create and export the route tree

export const routeTree = rootRoute.addChildren({
  IndexRoute,
  AuthorsIdRoute,
  UsersIdRoute,
  UsersSearchRoute,
  SearchIndexRoute,
  AuthorsEditIdRoute,
  EntriesBooksIdRoute,
  UsersEditIdRoute,
  AuthorsCreateIndexRoute,
  EntriesBooksEditIdRoute,
  EntriesBooksCreateIndexRoute,
})

/* prettier-ignore-end */

/* ROUTE_MANIFEST_START
{
  "routes": {
    "__root__": {
      "filePath": "__root.tsx",
      "children": [
        "/",
        "/authors/$id",
        "/users/$id",
        "/users/search",
        "/search/",
        "/authors/edit/$id",
        "/entries/books/$id",
        "/users/edit/$id",
        "/authors/create/",
        "/entries/books/edit/$id",
        "/entries/books/create/"
      ]
    },
    "/": {
      "filePath": "index.tsx"
    },
    "/authors/$id": {
      "filePath": "authors/$id.tsx"
    },
    "/users/$id": {
      "filePath": "users/$id.tsx"
    },
    "/users/search": {
      "filePath": "users/search.tsx"
    },
    "/search/": {
      "filePath": "search/index.tsx"
    },
    "/authors/edit/$id": {
      "filePath": "authors/edit/$id.tsx"
    },
    "/entries/books/$id": {
      "filePath": "entries/books/$id.tsx"
    },
    "/users/edit/$id": {
      "filePath": "users/edit/$id.tsx"
    },
    "/authors/create/": {
      "filePath": "authors/create/index.tsx"
    },
    "/entries/books/edit/$id": {
      "filePath": "entries/books/edit/$id.tsx"
    },
    "/entries/books/create/": {
      "filePath": "entries/books/create/index.tsx"
    }
  }
}
ROUTE_MANIFEST_END */
