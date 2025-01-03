import { FunctionComponent } from 'react'
import { EntryAuthor } from '../../../models/entry/Entry.ts'
import { useNavigate } from '@tanstack/react-router'
import { Author } from '../../../models/author/Author.ts'

interface Props {
  author: EntryAuthor | Author
}

const AuthorPreview: FunctionComponent<Props> = ({ author }) => {
  const imgSrc = 'data:image/*;base64,' + author.profilePicture
  const navigate = useNavigate()
  const handleLink = async () => {
    await navigate({ to: `/authors/${author.id}` })
  }

  return (
    <div
      className="flex max-w-36 cursor-pointer flex-col justify-between"
      onClick={handleLink}
    >
      <img
        src={imgSrc}
        className="aspect-square h-36 w-36 rounded-full border-4 border-white bg-slate-300 p-2"
        alt="cover photo"
      />
      <div className="flex flex-row justify-center font-bold">
        {author.name} {author.surname}
      </div>
      <div className="flex flex-row justify-center">
        {'details' in author ? author.details : ""}
      </div>
    </div>
  )
}

export default AuthorPreview
