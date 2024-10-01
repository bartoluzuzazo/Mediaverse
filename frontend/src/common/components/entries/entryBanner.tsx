import { FunctionComponent } from 'react'
import { Entry } from '../../../models/entry/Entry.ts'
import EntryType from './entryType.tsx'
import CustomImage from '../customImage'

interface props {
  entry: Entry
  info: string[]
  type: string
}

const EntryBanner: FunctionComponent<props> = ({ entry, info, type }) => {
  const imgSrc = 'data:image/*;base64,' + entry.photo

  return (
    <>
      <div className="-mx-[calc(50vw-50%)] h-[237px] bg-violet-100">
        <div className="flex items-center justify-evenly">
          <div className="h-[235px] w-[170px] p-2">
            <CustomImage
              className="h-full min-h-16 w-full object-cover"
              src={imgSrc}
            />
          </div>
          <div className="flex flex-col">
            <div className="center flex flex-row items-center p-4 font-bold">
              <h1 className="font-italic p-2">{entry.name} </h1>
              <div className="p-2">
                <EntryType type={type} />
              </div>
            </div>
            <div className="flex flex-col p-4 md:flex-row">
              <div className="font-bold text-slate-700">
                <h4>{info.join(' • ')}</h4>
              </div>
            </div>
          </div>
          <div className="text-5xl font-bold">{entry.ratingAvg}★</div>
        </div>
      </div>
    </>
  )
}

export default EntryBanner
