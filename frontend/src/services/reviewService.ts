import { Review, ReviewFormData } from '../models/review'
import axios from 'axios'

export class reviewService {
  public static async putReview(entryId: string, review: ReviewFormData) {
    return await axios.put<Review>(
      `/entry/${entryId}/reviews/current-user`,
      review
    )
  }
  public static async getReview(entryId: string, userId: string) {
    return await axios.get<Review>(`/entry/${entryId}/reviews/${userId}`)
  }
}
