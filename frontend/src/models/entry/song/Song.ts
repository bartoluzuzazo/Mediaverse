import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { EntryPreview } from '../../author/Author.ts'

export interface Song {
  entry: Entry,
  lyrics: string,
  musicGenres: string[],
  workOns: WorkOn[],
  albums: EntryPreview[],
}