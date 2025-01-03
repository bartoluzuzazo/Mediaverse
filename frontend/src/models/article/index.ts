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
