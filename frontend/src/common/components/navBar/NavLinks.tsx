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
        <span className="text-mv-slate">
            Find users by username
          </span>
      </li>
      <AuthorizedView>
        <li className="flex flex-col">
          <Link
            to={'/users/edit/$id'}
            params={{ id: authUserData!.id }}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Edit User Profile
          </Link>
          <span className="text-mv-slate">
            Change your profile picture, email or password.
          </span>
        </li>
      </AuthorizedView>
      <AuthorizedView allowedRoles={'Administrator'}>
        <li className="flex flex-col">
          <Link
            to={'/authors/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline"
          >
            Add author
          </Link>
          <span className="text-mv-slate">
            Add information about a new artist
          </span>
        </li>
        <li className="flex flex-col">
          <Link
            to={'/entries/books/create'}
            className="font-semibold text-black hover:text-mv-purple hover:underline">
            Add Book
          </Link>
          <span className="text-mv-slate">Add information about a new book</span>
        </li>
      </AuthorizedView>
    </ul>
  )
}
