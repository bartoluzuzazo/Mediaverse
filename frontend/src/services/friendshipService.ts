import axios from 'axios'
import { Friendship } from '../models/friendship'

export class friendshipService {
  public static async getFriendship(friendId: string) {
    return await axios.get<Friendship>(`/friendships/${friendId}`)
  }

  public static async postInvitation(friendId: string) {
    return await axios.post(`friendships/${friendId}`)
  }
  public static async postFriendshipApproval(friendId: string) {
    return await axios.post(`friendships/${friendId}/approval`)
  }
  public static async deleteFriendship(friendId: string) {
    return await axios.delete(`friendships/${friendId}`)
  }
}
