import { CreateAuthorCommand } from '../models/author/CreateAuthorCommand.ts'
import axios from 'axios'

export class AuthorService {
  public static async postAuthor(author: CreateAuthorCommand) {
    const formData = new FormData()
    formData.append('name', author.name)
    formData.append('surname', author.surname)
    formData.append('bio', author.bio)
    formData.append('profilePicture', author.profilePicture)
    return await axios.post('/authors', formData)
  }
}
