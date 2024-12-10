export interface ReviewFormData {
  id?: string
  title: string
  content: string
  grade: number
}

export interface Review {
  userId: string
  username: string
  profilePicture: string
  entryId: string
  content: string
  title: string
  grade: number
  entryTitle: string
  coverPhoto: string
}
