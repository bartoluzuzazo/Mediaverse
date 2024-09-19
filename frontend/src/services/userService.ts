import axios from 'axios'
import { User, UserFormData } from '../models/user'

export class userService {
  public static async getUser(id: string) {
    return await axios.get<User>(`/user/${id}`)
  }

  public static async getUserByEmail(email: string) {
    return await axios.get(`/user/email/${email}`)
  }

  public static async patchUser(user: UserFormData) {
    return await axios.patch<{ token: string }>(`/user/current-user`, user)
  }
}
