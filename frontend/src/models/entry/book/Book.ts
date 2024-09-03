import { Entry } from '../Entry.ts'

export interface Book {
  id: string,
  entry: Entry
  isbn: string,
  synopsis: string,
  bookGenres: string[]
}