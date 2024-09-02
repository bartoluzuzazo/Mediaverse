import axios from 'axios'
import { Login } from '../models/authentication/login'
import { Register } from '../models/authentication/register'
import { BaseResponse } from '../models/common'

export class AuthService {
  public static async login(data: Login) {
    return await axios.post<{ token: string }>('/user/login', data)
  }

  public static async register(data: Register) {
    return await axios.post<BaseResponse>('/user/register', data)
  }
}
