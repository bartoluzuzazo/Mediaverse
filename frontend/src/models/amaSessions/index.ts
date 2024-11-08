import { OrderDirection, PaginateRequest } from '../common'

export interface AmaSessionFormData {
  title: string
  description: string
  start: string
  end: string
  authorId: string
}

export interface AmaSession {
  id: string
  start: string
  end: string
  authorName: string
  authorSurname: string
  profilePicture: string
  title: string
  description: string
}

export interface AmaQuestionFormData {
  content: string
}

export enum AmaQuestionOrder {
  TotalVotes,
}

export enum AmaQuestionStatus {
  All,
  Answered,
  Unanswered,
}

export interface GetAmaQuestionParams extends PaginateRequest {
  order: AmaQuestionOrder
  status: AmaQuestionStatus
  direction: OrderDirection
}

export interface AmaQuestion {
  id: string
  amaSessionId: string
  userId: string
  username: string
  profilePicture: string
  content: string
  answer?: string
  likes: number
  likedByUser: boolean
}
