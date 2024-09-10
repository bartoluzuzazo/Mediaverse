import axios from 'axios'
import { Page } from '../models/common'
import { GetCommentsParams, Vote } from '../models/comments'
import { Comment } from '../models/comments'
import { CommentFormData } from '../common/components/comments/CommentForm.tsx'

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

  public static async postRootComment(comment: CommentFormData) {
    return await axios.post(`entries/${comment.entryId}/comments`, comment)
  }

  public static async postVote(vote: Vote) {
    return await axios.post(`comments/${vote.commentId}/votes`, vote)
  }
  public static async putVote(vote: Vote) {
    return await axios.put(
      `comments/${vote.commentId}/votes/current-user`,
      vote
    )
  }
  public static async deleteVote(commentId: string) {
    return await axios.delete(`comments/${commentId}/votes/current-user`)
  }
}
