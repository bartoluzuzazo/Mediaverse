import { FunctionComponent } from 'react'
import { Entry } from '../../../models/entry/Entry.ts'
import EntryType from './entryType.tsx'

interface props {
  entry: Entry,
  info: string[],
  type: string
}

const EntryBanner: FunctionComponent<props> = ({ entry, info, type }) => {
  return (
    <>
      <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-100 md:h-40">
        <div className="flex justify-around items-center">
          <div className="flex flex-col">
            <div className="font-bold flex-row flex center p-4 items-center">
              <h1 className="font-italic p-2">
                {entry.name}{' '}
              </h1>
              <div className="p-2">
                <EntryType type={type} />
              </div>
            </div>
            <div className="flex flex-col p-4 md:flex-row">
              <div className="text-slate-700 font-bold">
                <h4>{info.join(' • ')}</h4>
              </div>
            </div>
          </div>
          <div className="text-5xl font-bold">
            {entry.ratingAvg}★
          </div>
        </div>
      </div>
    </>
  )
}

export default EntryBanner