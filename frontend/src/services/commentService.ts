import axios from 'axios'
import { Page } from '../models/common'
import { Comment, CommentFormData, GetCommentsParams, Vote } from '../models/comments'

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

  public static async getSubcommentsAuthorized(
    commentId: string,
    params: GetCommentsParams
  ) {
    return await axios.get<Page<Comment>>(
      `comments/${commentId}/authorized-sub-comments`,
      { params }
    )
  }

  public static async getSubcommentsUnauthorized(
    commentId: string,
    params: GetCommentsParams
  ) {
    return await axios.get<Page<Comment>>(
      `comments/${commentId}/sub-comments`,
      { params }
    )
  }

  public static async postRootComment(comment: CommentFormData) {
    return await axios.post(`entries/${comment.entryId}/comments`, comment)
  }

  public static async postSubcomment(comment: CommentFormData) {
    return await axios.post(
      `comments/${comment.commentId}/sub-comments`,
      comment
    )
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

  public static async deleteComment(commentId: string){
    return await axios.delete(`comments/${commentId}`)
  }
}
