import { FunctionComponent } from 'react'
import { Link } from '@tanstack/react-router'
import { useAuthContext } from '../../../context/auth/useAuthContext.ts'
import { AuthorizedView } from '../auth/AuthorizedView'

export const NavLinks: FunctionComponent = () => {
  const { authUserData } = useAuthContext()!

  return (
    <ul className="flex flex-col gap-2">
      <li className="flex flex-col">
        <Link
          to={'/users/search'}
          className="font-semibold text-black hover:text-mv-purple hover:underline"
        >
          Search users
        </Link>
        <span className="text-mv-slate">Find users by username</span>
      </li>
      {authUserData && (
        <AuthorizedView>
          <li className="flex flex-col">
            <Link
              to={'/users/$id'}
              params={{ id: authUserData.id }}
              className="font-semibold text-black hover:text-mv-purple hover:underline"
            >
              Your profile
            </Link>
            <span className="text-mv-slate">View your profile</span>
          </li>
        </AuthorizedView>
      )}
      <AuthorizedView allowedRoles={'Administrator'}>
        <li className="flex flex-col">
          <Link
            to={'/authors/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Author
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/books/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Book
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/movies/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Movie
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/games/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Game
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/songs/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Song
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/albums/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Album
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/songs/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Song
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/series/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Series
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/songs/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Song
          </Link>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/episodes/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add Episode
          </Link>
        </li>
      </AuthorizedView>
    </ul>
  )
}
