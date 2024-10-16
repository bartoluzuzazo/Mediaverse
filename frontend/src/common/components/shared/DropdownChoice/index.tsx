import { FunctionComponent, ReactNode, useState } from 'react'
import { useToggle } from 'usehooks-ts'

interface DropDownOption<T> {
  element: ReactNode
  value: T
}

interface Props<T> {
  options: DropDownOption<T>[]
  defaultIndex?: number
  onChange: (value: T) => void | Promise<void>
  containerClassName?: string
}

export const DropDownChoice = <T extends unknown>({
                                                    options,
                                                    onChange,
                                                    defaultIndex = 0,
                                                    containerClassName,
                                                  }: Props<T>) => {
  const [isOpen, toggleIsOpen] = useToggle()
  const [chosenOption, setChosenOption] = useState(options[defaultIndex])
  return (
    <div className='relative'>
      <button onClick={toggleIsOpen} className='p-0 outline-none border-none'>
        {chosenOption.element}
      </button>
      {isOpen &&
        <div className={`absolute ${containerClassName} bg-white rounded-md shadow-lg p-2.5 border border-solid border-slate-200 z-20 space-y-2`}>
          {options.filter(o => o.value != chosenOption.value).map((o,i) => {
            return (
              <button  key={i}
                className='p-0 outline-none border-none w-full'
                      onClick={() => {
                        onChange(o.value)
                        setChosenOption(o)
                        toggleIsOpen()
                      }}>
                {o.element}
              </button>
            )
          })}
        </div>
      }
    </div>
  )
}

interface DropdownButtonProps {
  icon: ReactNode
  text: string
}

export const DropdownButton: FunctionComponent<DropdownButtonProps> = ({ icon, text }) => {
  return (
    <div className='bg-slate-300 rounded-3xl flex items-center px-4 py-1 '>
      {icon} <span className='ml-2 text-slate-900 text-lg font-semibold'>{text}</span>
    </div>
  )
}