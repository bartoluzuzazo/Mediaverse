import axios from 'axios'
import { GetRatedEntryPageRequest, RatedEntry } from '../models/entry/ratedEntry'
import { UpdatePasswordFormData, User, UserFormData } from '../models/user'
import { Page, PaginateRequest } from '../models/common'
import { RoleStatus } from '../models/user/role'

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
    params: GetRatedEntryPageRequest,
  ) {
    return await axios.get<Page<RatedEntry>>(`user/${userId}/rated-entries`, {
      params,
    })
  }

  public static async getFriends(userId: string) {
    return await axios.get<User[]>(`user/${userId}/friends`)
  }

  public static async putPassword(updatePasswordData: UpdatePasswordFormData) {
    return await axios.put('user/current-user/password', updatePasswordData)
  }

  public static async getRoleStatuses(userId: string) {
    return await axios.get<RoleStatus[]>(`/user/${userId}/role-statuses`)
  }

  public static async postUsersRole(userId: string, roleId: string) {
    return await axios.post(`/user/${userId}/roles`, { roleId })
  }

  public static async deleteUsersRole(userId: string, roleId: string) {
    return await axios.delete(`/user/${userId}/roles/${roleId}`)
  }

  public static async search(query: string, params: PaginateRequest) {
    return await axios.get<Page<User>>(`user/search`, { params: { ...params, query } })
  }
}
