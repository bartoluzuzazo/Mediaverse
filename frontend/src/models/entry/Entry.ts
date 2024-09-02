import { Rating } from './rating/Rating.ts'

export interface Entry {
  id: string
  name: string
  description: string
  photo: string
  release: Date
  ratingAvg: number
  usersRating?: Rating
}
<<<<<<< HEAD

export enum EntryOrder {
  Rating,
  Name,
  Release,
  Id,
}
=======
>>>>>>> origin/main
