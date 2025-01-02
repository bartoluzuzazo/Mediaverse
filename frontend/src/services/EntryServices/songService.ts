import axios from 'axios'

import { Entry } from '../../models/entry/Entry.ts'
import { Song } from '../../models/entry/song/Song.ts'
import { SongFormData } from '../../common/components/entries/songs/SongForm.tsx'
import { GetEntryPageRequest } from '../../models/entry/book'

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
}

