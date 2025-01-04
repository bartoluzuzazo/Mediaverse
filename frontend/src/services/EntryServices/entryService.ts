import axios from 'axios'
import { GetEntriesResponse } from '../../models/entry/Entry.ts'

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

  public static async getEntryType(id: string) : Promise<string>{
    return (
      await axios.get<string>(`entry/type/${id}`)
    ).data
  }
}
