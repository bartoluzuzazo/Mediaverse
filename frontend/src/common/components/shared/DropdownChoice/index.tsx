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
    <div className="relative">
      <button
        onClick={toggleIsOpen}
        className="border-none bg-transparent p-0 outline-none"
      >
        {chosenOption.element}
      </button>
      {isOpen && (
        <div
          className={`absolute ${containerClassName} z-20 space-y-2 rounded-md border border-solid border-slate-200 bg-white p-2.5 shadow-lg`}
        >
          {options
            .filter((o) => o.value != chosenOption.value)
            .map((o, i) => {
              return (
                <button
                  key={i}
                  className="w-full border-none bg-transparent p-0 outline-none"
                  onClick={() => {
                    onChange(o.value)
                    setChosenOption(o)
                    toggleIsOpen()
                  }}
                >
                  {o.element}
                </button>
              )
            })}
        </div>
      )}
    </div>
  )
}

interface DropdownButtonProps {
  icon: ReactNode
  text: string
}

export const DropdownButton: FunctionComponent<DropdownButtonProps> = ({
  icon,
  text,
}) => {
  return (
    <div className="flex items-center rounded-3xl bg-slate-300 px-4 py-1">
      {icon}{' '}
      <span className="ml-2 whitespace-nowrap text-lg font-semibold text-slate-900">
        {text}
      </span>
    </div>
  )
}
