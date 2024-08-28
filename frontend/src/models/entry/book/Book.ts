import { Entry } from '../Entry.ts'

export interface Book {
  entry: Entry
  isbn: string,
  synopsis: string,
  bookGenres: string[]
}