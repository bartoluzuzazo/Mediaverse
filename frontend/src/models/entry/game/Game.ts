import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'

export interface Game {
  entry: Entry
  synopsis: string,
  gameGenres: string[]
  workOns: WorkOn[]
}