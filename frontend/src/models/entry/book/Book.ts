import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'

export interface Book {
  id: string,
  entry: Entry
  isbn: string,
  synopsis: string,
  bookGenres: string[]
  workOns: WorkOn[]
}

