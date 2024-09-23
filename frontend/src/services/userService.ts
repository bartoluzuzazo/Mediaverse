import axios from 'axios'
import {
  GetRatedEntryPageRequest,
  RatedEntry,
} from '../models/entry/ratedEntry'
import { User, UserFormData } from '../models/user'
import { Page } from '../models/common'

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

  public static async getFriendInvites() {
    return await axios.get<User[]>('/user/current-user/friend-invites')
  }

  public static async getRatedEntries(
    userId: string,
    params: GetRatedEntryPageRequest
  ) {
    return await axios.get<Page<RatedEntry>>(`user/${userId}/rated-entries`, {
      params,
    })
  }

  public static async getFriends(userId: string) {
    return await axios.get<User[]>(`user/${userId}/friends`)
  }
}
