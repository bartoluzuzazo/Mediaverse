import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'

export interface Movie {
  entry: Entry
  synopsis: string,
  cinematicGenres: string[]
  workOns: WorkOn[]
}