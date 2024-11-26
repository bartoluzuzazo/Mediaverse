import { Link, useNavigate } from '@tanstack/react-router'
import { FunctionComponent, useEffect, useState } from 'react'
import { FaSearch } from 'react-icons/fa'
import { RxHamburgerMenu } from 'react-icons/rx'
import { useAuthContext } from '../../../context/auth/useAuthContext'
import FormButton from '../form/button'
import FormInput from '../form/input'
import AuthPanel from './authPanel'
import NavBarBurger from './navBarBurger'
import { useSearchPanelContext } from '../../../context/searchPanel'
import { useForm } from 'react-hook-form'

interface NavBarProps {}

const NavBar: FunctionComponent<NavBarProps> = () => {
  const [isBurgerOpen, setBurgerOpen] = useState(false)
  const [isAuthPanelOpen, setAuthPanelOpen] = useState(false)

  const searchPanelContext = useSearchPanelContext()

  const authContext = useAuthContext()

  useEffect(() => {
    if (authContext?.token) {
      setAuthPanelOpen(false)
    }
  }, [authContext?.token])

  const { handleSubmit } = useForm()

  const navigate = useNavigate()

  return (
    <div className="z-10 flex h-[90px] w-full items-center justify-between px-[40px] shadow-xl">
      <div className="relative flex w-[200px] items-center gap-6">
        <NavBarBurger isOpen={isBurgerOpen} setIsOpen={setBurgerOpen} />
        <RxHamburgerMenu
          className="burger-icon size-7 pt-1 text-mv-gray hover:cursor-pointer"
          onClick={() => setBurgerOpen((prev) => !prev)}
        />
        <Link
          className="select-none text-2xl font-semibold text-mv-purple visited:text-mv-purple hover:text-mv-purple"
          to="/"
        >
          MediaVerse
        </Link>
      </div>
      <div className="w-[600px]">
        <form
          onSubmit={handleSubmit(() => {
            navigate({
              to: '/search',
              search: { searchQuery: searchPanelContext?.searchValue || '' },
            })
            searchPanelContext?.setSearchValue('')
          })}
        >
          <FormInput
            inputProps={{
              type: 'text',
              placeholder: 'search...',
              value: searchPanelContext?.searchValue || '',
              onChange: (e) =>
                searchPanelContext?.setSearchValue?.(e.target.value),
            }}
            rightElement={<FaSearch />}
          />
        </form>
      </div>
      <div className="relative w-[200px]">
        {authContext?.token ? (
          <FormButton
            buttonProps={{
              onClick: () => {
                authContext?.removeToken()
              },
            }}
          >
            Sign out
          </FormButton>
        ) : (
          <FormButton buttonProps={{ onClick: () => setAuthPanelOpen(true) }}>
            Sign in
          </FormButton>
        )}
        <AuthPanel isOpen={isAuthPanelOpen} />
      </div>
    </div>
  )
}

export default NavBar
