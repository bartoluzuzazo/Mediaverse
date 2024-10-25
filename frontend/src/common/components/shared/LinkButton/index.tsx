import { AnchorHTMLAttributes, forwardRef, ReactNode } from 'react'
import { createLink, LinkComponent } from '@tanstack/react-router'

interface LinkButtonProps extends AnchorHTMLAttributes<HTMLAnchorElement> {
  icon: ReactNode
}


const BasicLinkButton = forwardRef<HTMLAnchorElement, LinkButtonProps>(
  ({children, icon, ...rest}, ref)=>{

    return(

    <a ref={ref}
      {...rest}
      className="mt-2 flex w-full items-center gap-3 rounded-md bg-violet-200 p-1 text-black hover:text-black hover:scale-105">
      <div className="m-1 grid aspect-square w-10 place-content-center self-center rounded-full bg-violet-900 text-white">
        {icon}
      </div>
      <span className="text-lg font-semibold">{children}</span>
    </a>

    )
  }
)

const CreatedLinkButton = createLink(BasicLinkButton)

export const LinkButton: LinkComponent<typeof BasicLinkButton> = (props) => {
  return <CreatedLinkButton  preload={'intent'} {...props}/>
}