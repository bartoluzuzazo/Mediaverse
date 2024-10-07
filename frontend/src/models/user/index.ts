export interface User {
  id: string
  username: string
  profilePicture: string
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

export type Role = "Administrator" | "User" | "Author" | "ContentCreator" // other roles to be added at a later date
