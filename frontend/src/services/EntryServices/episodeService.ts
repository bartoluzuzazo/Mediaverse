import axios from 'axios'

import { Entry, EntrySearch } from '../../models/entry/Entry.ts'
import { Episode } from '../../models/entry/episode/Episode.ts'
import { GetEntryPageRequest } from '../../models/entry/book'
import { EpisodeFormData } from '../../common/components/entries/episodes/EpisodeForm.tsx'
import { Page, PaginateRequest } from '../../models/common'

export class EpisodeService {
  public static async getEpisode(id: string) {
    return await axios.get<Episode>(`/episode/${id}`)
  }

  public static async getEpisodePage(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('episode/page', { params })
  }

  public static async postEpisode(episode: EpisodeFormData) {
    return await axios.post('/episode', episode)
  }

  public static async postEpisodes(id: string, episodes: EpisodeFormData[]) {
    return await axios.post(`/episode/${id}`, episodes)
  }

  public static async patchEpisode(episode: EpisodeFormData, id: string) {
    return await axios.patch(`/episode/${id}`, episode)
  }

  public static async search(query: string, params: PaginateRequest) {
    return await axios.get<Page<EntrySearch>>(`/episode/search`, { params: { ...params, query } })
  }
}

