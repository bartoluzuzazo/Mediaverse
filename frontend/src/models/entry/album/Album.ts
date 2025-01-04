import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { EntryPreview } from '../../author/Author.ts'

export interface Album {
  entry: Entry,
  musicGenres: string[],
  workOns: WorkOn[],
  songs: EntryPreview[]
}
