export interface Entry {
  id: string
  name: string
  description: string
  photo: string
  release: Date
  ratingAvg: number
  usersRating?: Rating
}

export enum EntryOrder {
  Rating,
  Name,
  Release,
  Id,
}
