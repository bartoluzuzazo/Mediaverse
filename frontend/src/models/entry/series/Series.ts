import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { EntryPreview } from '../../author/Author.ts'

export interface Series {
  entry: Entry,
  cinematicGenres: string[],
  seasons: SeriesSeason[],
  workOns: WorkOn[]
}

export interface SeriesSeason {
  seasonNumber: number,
  episodes: EntryPreview[]
}