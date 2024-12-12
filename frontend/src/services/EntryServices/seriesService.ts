import axios from 'axios'

import { Entry } from '../../models/entry/Entry.ts'
import { Series } from '../../models/entry/series/Series.ts'
import { GetEntryPageRequest } from '../../models/entry/book'
import { SeriesFormData } from '../../common/components/entries/series/SeriesForm.tsx'

export class SeriesService {
  public static async getSeries(id: string) {
    return await axios.get<Series>(`/series/${id}`)
  }

  public static async getSeriesPage(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('series/page', { params })
  }

  public static async postSeries(series: SeriesFormData) {
    return await axios.post('/series', series)
  }

  public static async patchSeries(series: SeriesFormData, id: string) {
    return await axios.patch(`/series/${id}`, series)
  }
}

