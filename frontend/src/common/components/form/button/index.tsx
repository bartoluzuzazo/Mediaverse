import classNames from 'classnames'
import { FunctionComponent } from 'react'

type ButtonStyleClass = { textColor: string; bgColor: string }

const formButtonTypes = {
  normal: { textColor: 'text-white', bgColor: 'bg-black' },
  red: { textColor: 'text-white', bgColor: 'bg-mv-red' },
  purple: { textColor: 'text-white', bgColor: 'bg-mv-light-purple' },
} satisfies Record<string, ButtonStyleClass>

interface FormButtonProps {
  children?: React.ReactNode
  buttonProps?: React.DetailedHTMLProps<
    React.ButtonHTMLAttributes<HTMLButtonElement>,
    HTMLButtonElement
  >
  buttonType?: keyof typeof formButtonTypes
}

const FormButton: FunctionComponent<FormButtonProps> = ({
  children,
  buttonProps,
  buttonType = 'normal',
}) => {
  return (
    <button
      className={classNames(
        'h-[45px]',
        formButtonTypes[buttonType].bgColor,
        formButtonTypes[buttonType].textColor
      )}
      {...buttonProps}
    >
      {children}
    </button>
  )
}

export default FormButton
