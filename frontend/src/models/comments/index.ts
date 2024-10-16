import { OrderDirection, PaginateRequest } from '../common'

export interface Comment {
  id: string
  entryId: string
  username: string
  userProfile?: string
  userId?: string
  content: string
  subcommentCount: number
  upvotes: number
  downvotes: number
  usersVote?: boolean
  isDeleted: boolean
  createdAt?: string
}
export enum CommentOrder {
  votes,
  voteCount,
  createdAt
}

export interface GetCommentsParams extends PaginateRequest {
  order: CommentOrder
  direction: OrderDirection
}

export interface Vote {
  isPositive: boolean
  commentId: string
}
export interface CommentFormData {
  content: string
  entryId: string
  commentId?: string
}
