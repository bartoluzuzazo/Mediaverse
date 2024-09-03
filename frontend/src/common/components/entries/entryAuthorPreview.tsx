import { FunctionComponent } from 'react'
import { EntryAuthor } from '../../../models/entry/Entry.ts'
import { useNavigate } from '@tanstack/react-router'

interface Props{
  author: EntryAuthor
}

const AuthorEntryPreview : FunctionComponent<Props> = ({author}) => {
  const imgSrc = 'data:image/*;base64,' + author.profilePicture
  const navigate = useNavigate();
  const handleLink = async () => {
    await navigate({ to: `/authors/${author.id}` })
  }

  return (
    <div className="flex flex-col justify-between max-w-36 cursor-pointer" onClick={handleLink}>
      <img
        src={imgSrc}
        className="aspect-square w-36 h-36 rounded-full border-4 border-white bg-slate-300 p-2"
        alt="cover photo"
      />
      <div className="flex flex-row justify-center font-bold">
        {author.name} {author.surname}
      </div>
    </div>
  )
}

export default AuthorEntryPreview;