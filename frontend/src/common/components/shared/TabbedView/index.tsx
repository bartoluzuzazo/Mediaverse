import { FunctionComponent, ReactNode, useState } from 'react'

interface Tab {
  key: string
  element: ReactNode
}
type Props = {
  tabs: Tab[]
}

export const TabbedView: FunctionComponent<Props> = ({ tabs }) => {
  const [chosenTab, setChosenTab] = useState(tabs[0])
  return (
    <div>
      <div className="my-3 flex gap-3 overflow-auto">
        {tabs.map((t) => {
          return (
            <button
              key={t.key}
              className={`block border-2 border-violet-800 px-3 py-1.5 ${t.key === chosenTab.key ? 'bg-violet-800 text-white' : 'bg-white text-violet-800'}`}
              onClick={() => setChosenTab(t)}
            >
              {t.key}
            </button>
          )
        })}
      </div>
      {chosenTab.element}
    </div>
  )
}
