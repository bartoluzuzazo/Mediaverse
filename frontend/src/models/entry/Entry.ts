import { Rating } from './rating/Rating.ts'

export interface Entry {
  id: string
  name: string
  description: string
  photo: string
  release: Date
  ratingAvg: number
  usersRating?: Rating
  authors: EntryWorkOnGroup[]
}

export enum EntryOrder {
  Rating,
  Name,
  Release,
  Id,
}

interface EntryWorkOnGroup{
  role: string,
  authors: EntryAuthor[]
}

export interface EntryAuthor{
  id: string,
  name: string,
  surname: string,
  profilePicture: string
}