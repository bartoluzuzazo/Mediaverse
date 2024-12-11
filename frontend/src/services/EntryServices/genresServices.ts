import axios from 'axios'

import { Page, PaginateRequest } from '../../models/common'

export class GenresServices {
  public static async searchBookGenres(query: string, params: PaginateRequest) {
    return await axios.get<Page<string>>(`/genres/search/book`, { params: { ...params, query } })
  }
  public static async searchCinematicGenres(query: string, params: PaginateRequest) {
    return await axios.get<Page<string>>(`/genres/search/cinematic`, { params: { ...params, query } })
  }
  public static async searchGameGenres(query: string, params: PaginateRequest) {
    return await axios.get<Page<string>>(`/genres/search/game`, { params: { ...params, query } })
  }
}

