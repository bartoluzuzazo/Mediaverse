import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { EntryPreview } from '../../author/Author.ts'

export interface Episode {
  entry: Entry,
  synopsis: string,
  seasonNumber: number,
  episodeNumber: number,
  workOns: WorkOn[],
  series: EntryPreview
}
