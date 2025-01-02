import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'

export interface Song {
  entry: Entry,
  lyrics: string,
  musicGenres: string[],
  workOns: WorkOn[]
}