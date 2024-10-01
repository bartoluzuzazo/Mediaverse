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
