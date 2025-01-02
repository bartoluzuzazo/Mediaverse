import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { Song } from '../song/Song.ts'

export interface Album {
  entry: Entry,
  musicGenres: string[],
  workOns: WorkOn[],
  songs: Song[]
}