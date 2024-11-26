import axios from 'axios'
import { GetEntriesResponse } from '../models/entry/Entry'

export class EntryService {
  public static async getEntries(
    searchTerm: string,
    page?: number,
    type?: string
  ) {
    return (
      await axios.get<GetEntriesResponse>('entry', {
        params: { searchTerm: `'${searchTerm}'`, page, type },
      })
    ).data
  }
}
