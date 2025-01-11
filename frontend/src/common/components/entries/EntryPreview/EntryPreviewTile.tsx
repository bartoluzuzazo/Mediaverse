import { FunctionComponent, MouseEventHandler } from 'react'
import { EntryPreview } from '../../../../models/author/Author.ts'
import EntryType from '../entryType.tsx'
import { Link } from '@tanstack/react-router'

interface Props {
  entry: EntryPreview
  onClick?: MouseEventHandler<HTMLDivElement>
}

const EntryPreviewTile: FunctionComponent<Props> = ({ entry }) => {
  const imgSrc = 'data:image/*;base64,' + entry.coverPhoto
  const maxDescLen = 400
  return (
    <Link
      className="flex cursor-pointer flex-row justify-between text-black"
      to={`/entries/${entry.id}`}
    >
      <img
        src={imgSrc}
        className="aspect-square h-36 w-36 border-4 border-white bg-slate-300 object-cover p-2"
        alt="cover photo"
      />
      <div className="flex flex-col p-2 font-bold">
        <div className="flex flex-row">
          {entry.orderNumber ? <h2 className="p-2">{entry.orderNumber}.</h2> : <></>}
          <h2 className="p-2">{entry.name}</h2>
        </div>
        <EntryType type={entry.type} />
        <div className="flex flex-row">
          <h2 className="p-2">{entry.releaseDate.toString()}</h2>
          <h2 className="p-2">{entry.avgRating.toFixed(2)}★</h2>
        </div>
      </div>
      <p className="w-8/12 p-2 font-normal">
        {entry.description.length > maxDescLen
          ? entry.description.substring(0, maxDescLen) + ' [...]'
          : entry.description}
      </p>
    </Link>
  )
}

export default EntryPreviewTile
