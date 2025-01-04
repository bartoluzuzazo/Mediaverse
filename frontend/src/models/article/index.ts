import { PaginateRequest } from '../common'

export interface ArticleFormData {
  id?: string
  title: string
  lede: string
  content: string
}
export interface Article {
  id: string
  title: string
  lede: string
  content: string
  userId: string
  authorUsername: string
  authorPicture: string
}

export interface ArticlePreview {
  id: string
  title: string
  userId: string
  authorUsername: string
  authorPicture: string
  timestamp: string
  lede: string
}

export interface SearchArticleParams extends PaginateRequest {
  searched: string
}
