import axios from 'axios'

import { Entry, EntrySearch } from '../../models/entry/Entry.ts'
import { Song } from '../../models/entry/song/Song.ts'
import { SongFormData } from '../../common/components/entries/songs/SongForm.tsx'
import { GetEntryPageRequest } from '../../models/entry/book'
import { Page, PaginateRequest } from '../../models/common'

export class SongService {
  public static async getSong(id: string) {
    return await axios.get<Song>(`/song/${id}`)
  }

  public static async getSongs(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('song/page', { params })
  }

  public static async postSong(song: SongFormData) {
    return await axios.post('/song', song)
  }

  public static async patchSong(song: SongFormData, id: string) {
    return await axios.patch(`/song/${id}`, song)
  }

  public static async search(query: string, params: PaginateRequest) {
    return await axios.get<Page<EntrySearch>>(`/song/search`, { params: { ...params, query } })
  }
}

