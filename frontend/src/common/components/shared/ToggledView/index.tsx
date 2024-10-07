import { FunctionComponent } from 'react'
import { TbTriangleInvertedFilled } from 'react-icons/tb'
import { useToggle } from 'usehooks-ts'

type Props = {
  children: React.ReactNode
  title: string
  containerClass?: string
}

export const ToggledView: FunctionComponent<Props> = ({ children, containerClass, title }) => {
  const [isOpen, toggleOpen] = useToggle()
  return (
    <div className={containerClass}>
      <div className="flex gap-2 items-baseline mb-2">
        <button type="button" onClick={toggleOpen} className="bg-transparent p-0 m-0 outline-none shadow-none">
          <TbTriangleInvertedFilled className={`text-lg transition-all ${!isOpen ? `-rotate-90` : ``}`}/>
        </button>
        <span className="font-semibold text-xl">{title}</span>
      </div>
      {isOpen &&
        <div className='relative'>
          <div className='absolute top-0 left-0 right-0 bg-white rounded-md border-[3px] border-slate-300 p-1 pl-[1.83rem]'>
            {children}
          </div>
        </div>
      }
    </div>
  )
}