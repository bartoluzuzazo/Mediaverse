export interface Author {
  id: string
  name: string
  surname: string
  bio: string
  workOns: AuthorWorkOnGroup[]
  profilePicture: string
  userId?: string
}

interface AuthorWorkOnGroup {
  role: string
  entries: EntryPreview[]
}

export interface EntryPreview {
  id: string
  name: string
  releaseDate: Date
  avgRating: number
  description: string
  type: string
  coverPhoto: string
}
