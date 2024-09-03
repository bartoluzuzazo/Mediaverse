export interface Author {
  id: string
  name: string
  surname: string
  bio: string
  workOns: WorkOnGroup[]
  profilePicture: string
}

interface WorkOnGroup {
  role: string,
  entries: AuthorEntry[]
}

export interface AuthorEntry {
  id: string,
  name: string,
  releaseDate: Date,
  avgRating: number,
  description: string,
  type: string,
  coverPhoto: string
}