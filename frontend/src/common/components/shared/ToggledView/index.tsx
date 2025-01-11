import { FunctionComponent } from 'react'
import { TbTriangleInvertedFilled } from 'react-icons/tb'
import { useToggle } from 'usehooks-ts'

type Props = {
  children: React.ReactNode
  title: string
  containerClass?: string
}

export const ToggledView: FunctionComponent<Props> = ({
  children,
  containerClass,
  title,
}) => {
  const [isOpen, toggleOpen] = useToggle()
  return (
    <div className={containerClass}>
      <button
        type="button"
        onClick={toggleOpen}
        className="mb-2 flex items-baseline gap-2 border-none bg-transparent p-0 outline-none"
      >
        <div className="m-0 bg-transparent p-0 shadow-none outline-none">
          <TbTriangleInvertedFilled
            className={`text-lg transition-all ${!isOpen ? `-rotate-90` : ``}`}
          />
        </div>
        <span className="font-semibold">{title}</span>
      </button>
      {isOpen && (
        <div className="relative">
          <div className="absolute left-0 right-0 top-0 rounded-md border border-solid border-slate-200 bg-white pb-2 pl-[1.83rem] pr-1.5 shadow-lg">
            {children}
          </div>
        </div>
      )}
    </div>
  )
}
