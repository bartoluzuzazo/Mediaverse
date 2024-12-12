import { Entry } from '../../Entry.ts'
import { WorkOn } from '../../WorkOn.ts'

export interface Episode {
  entry: Entry,
  synopsis: string,
  seriesId: string
  workOns: WorkOn[]
}

export interface EpisodePreview {
  id: string,
  name: string,
  episodeNumber: number,
  release: string,
  ratingAvg: number,
  synopsis: string
}