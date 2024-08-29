import { FunctionComponent, useEffect, useState } from 'react'
import FormInput from '../form/input'
import { FaSearch } from 'react-icons/fa'
import { RxHamburgerMenu } from 'react-icons/rx'
import FormButton from '../form/button'
import NavBarBurger from './navBarBurger'
import { Link } from '@tanstack/react-router'
import AuthPanel from './authPanel'
import { useLocalStorage } from 'usehooks-ts'
import axios from 'axios'

interface NavBarProps {}

const NavBar: FunctionComponent<NavBarProps> = () => {
  const [isBurgerOpen, setBurgerOpen] = useState(false)
  const [isAuthPanelOpen, setAuthPanelOpen] = useState(false)

  const [token, setToken] = useLocalStorage<string | undefined>(
    'token',
    undefined
  )

  useEffect(() => {
    if (token) {
      setAuthPanelOpen(false)
    }
  }, [token])

  return (
    <div className="flex h-[90px] w-full items-center justify-between px-[40px] shadow-xl">
      <div className="relative flex w-[200px] items-center gap-6">
        <NavBarBurger isOpen={isBurgerOpen} setIsOpen={setBurgerOpen} />
        <RxHamburgerMenu
          className="burger-icon size-7 pt-1 text-mv-gray hover:cursor-pointer"
          onClick={() => setBurgerOpen((prev) => !prev)}
        />
        <Link className="select-none text-2xl font-semibold text-mv-purple visited:text-mv-purple hover:text-mv-purple">
          MediaVerse
        </Link>
      </div>
      <div className="w-[600px]">
        <FormInput
          inputProps={{ type: 'text', placeholder: 'search...' }}
          rightElement={<FaSearch />}
        />
      </div>
      <div className="relative w-[200px]">
        {token ? (
          <FormButton
            buttonProps={{
              onClick: () => {
                setToken('')
                axios.defaults.headers.common['Authorization'] = ''
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
