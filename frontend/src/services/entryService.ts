import axios from 'axios'
import { Entry } from '../models/entry/Entry'

export class EntryService {
  public static async getEntries(searchTerm: string) {
    return (
      await axios.get<{ data: Entry[] }>('entry', { params: { searchTerm } })
    ).data
  }
}
