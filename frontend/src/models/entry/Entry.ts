import { Author } from '../author/Author.ts'
import { Rating } from './rating/Rating.ts'
import { WorkOn } from './WorkOn.ts'

export interface Entry {
  id: string
  name: string
  description: string
  photo: string
  release: string
  ratingAvg: number
  usersRating?: Rating
  type?: string
  authors: EntryWorkOnGroup[]
  workOnRequests: WorkOn[]
}

export enum EntryOrder {
  Rating,
  Name,
  Release,
  Id,
}

interface EntryWorkOnGroup {
  role: string
  authors: EntryAuthor[]
}

export interface EntryAuthor {
  id: string
  name: string
  surname: string
  profilePicture: string
}

export interface GetEntriesResponse {
  data: {
    entries: Entry[]
    authros: Author[]
  }
}
