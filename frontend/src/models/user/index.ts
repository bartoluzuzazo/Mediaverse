import { EntryAuthor } from '../entry/Entry.ts'

export interface User {
  id: string
  username: string
  profilePicture: string

  authors?: EntryAuthor[]
}

export interface UserFormData {
  id: string
  email: string
  profilePicture?: string
}

export interface UpdatePasswordFormData {
  oldPassword: string
  newPassword: string
  repeatPassword: string
}

export type Role =
  | 'Administrator'
  | 'User'
  | 'Author'
  | 'ContentCreator'
  | 'Critic'
  | 'Journalist'// other roles to be added at a later date
