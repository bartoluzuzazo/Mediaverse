import { OrderDirection, PaginateRequest } from '../common'

export interface Comment {
  id: string
  entryId: string
  username: string
  userProfile?: string
  content: string
  subcommentCount: number
  upvotes: number
  downvotes: number
  usersVote?: boolean
}
export enum CommentOrder {
  votes,
  voteCount,
}

export interface GetCommentsParams extends PaginateRequest {
  order: CommentOrder
  direction: OrderDirection
}
