import axios from 'axios'

import { Entry, EntrySearch } from '../../models/entry/Entry.ts'
import { Album } from '../../models/entry/album/Album.ts'
import { AlbumFormData } from '../../common/components/entries/albums/AlbumForm.tsx'
import { GetEntryPageRequest } from '../../models/entry/book'
import { Page, PaginateRequest } from '../../models/common'

export class AlbumService {
  public static async getAlbum(id: string) {
    return await axios.get<Album>(`/album/${id}`)
  }

  public static async getAlbums(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('album/page', { params })
  }

  public static async postAlbum(album: AlbumFormData) {
    return await axios.post('/album', album)
  }

  public static async patchAlbum(album: AlbumFormData, id: string) {
    return await axios.patch(`/album/${id}`, album)
  }

  public static async search(query: string, params: PaginateRequest) {
    return await axios.get<Page<EntrySearch>>(`/album/search`, { params: { ...params, query } })
  }
}

