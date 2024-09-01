import axios from 'axios'
import { Rating } from '../models/entry/rating/Rating.ts'

export class ratingService {
  public static async getRating(entryId: string) {
    return await axios.get<Rating>(`/entries/${entryId}/ratings/users-rating`)
  }
  public static async postRating(entryId: string, rating: Rating) {
    return await axios.post(`/entries/${entryId}/ratings`, rating)
  }
  public static async putRating(ratingId: string, rating: Rating) {
    return await axios.put(`/ratings/${ratingId}`, rating)
  }
}
