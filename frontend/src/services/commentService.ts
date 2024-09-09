import axios from 'axios'
import { Page } from '../models/common'
import { GetCommentsParams } from '../models/comments'
import { Comment } from '../models/comments'

export class commentService {
  public static async getRootCommentsAuthorized(
    entryId: string,
    params: GetCommentsParams
  ) {
    return await axios.get<Page<Comment>>(
      `entries/${entryId}/authorized-comments`,
      { params }
    )
  }
  public static async getRootCommentsUnauthorized(
    entryId: string,
    params: GetCommentsParams
  ) {
    return await axios.get<Page<Comment>>(`entries/${entryId}/comments`, {
      params,
    })
  }
}
