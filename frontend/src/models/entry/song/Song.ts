import { Entry } from '../Entry.ts'
import { WorkOn } from '../WorkOn.ts'
import { Album } from '../album/Album.ts'

export interface Song {
  entry: Entry,
  lyrics: string,
  musicGenres: string[],
  workOns: WorkOn[],
  albums: Album[],
}