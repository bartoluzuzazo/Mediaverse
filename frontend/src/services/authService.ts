import axios from 'axios'

export class AuthService {
  public static async login(username: string, password: string) {
    return await axios.post('/user/login', { username, password })
  }

  public static async register(
    username: string,
    email: string,
    password: string
  ) {
    return await axios.post('/user/register', { username, email, password })
  }
}
