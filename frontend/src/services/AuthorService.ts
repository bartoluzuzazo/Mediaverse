import axios from 'axios'
import { AuthorFormData } from '../common/components/authors/AuthorForm.tsx'

export class AuthorService {
  public static async postAuthor(author: AuthorFormData) {
    return await axios.post('/authors', author)
  }

  public static async patchAuthor(author: AuthorFormData, id: string) {
    return await axios.patch(`/authors/${id}`, author)
  }

  public static async getAuthor(id: string) {
    return await axios.get(`/authors/${id}`)
  }
}
