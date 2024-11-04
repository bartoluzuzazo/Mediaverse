import { FunctionComponent, ReactNode, useState } from 'react'

interface Tab {
  key: string
  element: ReactNode
}
type Props = {
  tabs: Tab[]
}

export const TabbedView : FunctionComponent<Props> = ({tabs}) => {
  const [chosenTab, setChosenTab] = useState(tabs[0])
  return (
    <div>
      <div className="flex gap-3 overflow-auto">{tabs.map(t=>{
        return(
          <button key={t.key}
            className={`block px-3 py-1.5 border-2 border-violet-800 ${t.key === chosenTab.key ? 'text-white bg-violet-800': 'text-violet-800 bg-white' }`}
          onClick={()=>setChosenTab(t)}>
            {t.key}
          </button>
        )
      })}</div>
      {chosenTab.element}
    </div>
	)
}