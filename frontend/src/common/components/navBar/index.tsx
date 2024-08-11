import { FunctionComponent, useState } from 'react'
import FormInput from '../form/input'
import { FaSearch } from 'react-icons/fa'
import { RxHamburgerMenu } from 'react-icons/rx'
import FormButton from '../form/button'
import NavBarBurger from './navBarBurger'

interface NavBarProps {}

const NavBar: FunctionComponent<NavBarProps> = () => {
  const [isBurgerOpen, setBurgerOpen] = useState(false)

  return (
    <div className="flex h-[90px] w-full items-center justify-between px-[40px] shadow-xl">
      <div className="relative flex w-[200px] items-center gap-6">
        <NavBarBurger isOpen={isBurgerOpen} setIsOpen={setBurgerOpen} />
        <RxHamburgerMenu
          className="burger-icon text-mv-gray size-7 pt-1 hover:cursor-pointer"
          onClick={() => setBurgerOpen((prev) => !prev)}
        />
        <p className="text-mv-purple text-2xl font-semibold">MediaVerse</p>
      </div>
      <div className="w-[600px]">
        <FormInput
          inputProps={{ type: 'text', placeholder: 'search...' }}
          rightElement={<FaSearch />}
        />
      </div>
      <div className="w-[200px]">
        <FormButton>Sign in</FormButton>
      </div>
    </div>
  )
}

export default NavBar
