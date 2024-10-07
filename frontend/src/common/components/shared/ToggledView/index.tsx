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
      <button type="button" onClick={toggleOpen} className="flex gap-2 items-baseline mb-2 bg-transparent p-0 outline-none border-none">
        <div  className="bg-transparent p-0 m-0 outline-none shadow-none">
          <TbTriangleInvertedFilled className={`text-lg transition-all ${!isOpen ? `-rotate-90` : ``}`}/>
        </div>
        <span className="font-semibold text-xl">{title}</span>
      </button>
      {isOpen &&
        <div className='relative'>
          <div className='absolute top-0 left-0 right-0 bg-white rounded-md shadow-lg p-1 pl-[1.83rem] border border-solid border-slate-200'>
            {children}
          </div>
        </div>
      }
    </div>
  )
}