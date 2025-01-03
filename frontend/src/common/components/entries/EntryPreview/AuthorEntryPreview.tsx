import { FunctionComponent } from 'react'
import { EntryPreview } from '../../../../models/author/Author.ts'
import EntryType from '../entryType.tsx'
import { useNavigate } from '@tanstack/react-router'

interface Props {
  entry: EntryPreview
}

const AuthorEntryPreview: FunctionComponent<Props> = ({ entry }) => {
  const imgSrc = 'data:image/*;base64,' + entry.coverPhoto
  const maxDescLen = 400
  const navigate = useNavigate()
  const handleLink = async () => {
    await navigate({ to: `/entries/${entry.id}` })
  }
  return (
    <div
      className="flex cursor-pointer flex-row justify-between"
      onClick={handleLink}
    >
      <img
        src={imgSrc}
        className="aspect-square h-36 w-36 border-4 border-white bg-slate-300 p-2"
        alt="cover photo"
      />
      <div className="flex flex-col p-2 font-bold">
        <h2 className="p-2">{entry.name}</h2>
        <EntryType type={entry.type} />
        <div className="flex flex-row">
          <h2 className="p-2">{entry.releaseDate.toString()}</h2>
          <h2 className="p-2">{entry.avgRating.toFixed(2)}★</h2>
        </div>
      </div>
      <p className="w-8/12 p-2">
        {entry.description.length > maxDescLen
          ? entry.description.substring(0, maxDescLen) + ' [...]'
          : entry.description}
      </p>
    </div>
  )
}

export default AuthorEntryPreview
