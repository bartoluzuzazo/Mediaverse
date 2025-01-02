import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'

export interface Album {
  entry: Entry,
  musicGenres: string[],
  workOns: WorkOn[]
}