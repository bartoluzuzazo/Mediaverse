import axios from 'axios'

import { Entry } from '../../models/entry/Entry.ts'
import { Movie } from '../../models/entry/movie/Movie.ts'
import { MovieFormData } from '../../common/components/entries/movies/MovieForm.tsx'
import { GetEntryPageRequest } from '../../models/entry/book'

export class MovieService {
  public static async getMovie(id: string) {
    return await axios.get<Movie>(`/movie/${id}`)
  }

  public static async getMovies(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('movie/page', { params })
  }

  public static async postMovie(movie: MovieFormData) {
    return await axios.post('/movie', movie)
  }

  public static async patchMovie(movie: MovieFormData, id: string) {
    return await axios.patch(`/movie/${id}`, movie)
  }
}

