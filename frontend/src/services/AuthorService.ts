import axios from 'axios'
import { AuthorFormData } from '../common/components/authors/AuthorForm.tsx'

export class AuthorService {
  public static async postAuthor(author: AuthorFormData) {
    const formData = new FormData()
    formData.append('name', author.name)
    formData.append('surname', author.surname)
    formData.append('bio', author.bio)
    formData.append('profilePicture', author.profilePicture)
    return await axios.post('/authors', formData)
  }

  public static async patchAuthor(author: AuthorFormData, id: string) {
    const formData = new FormData()
    formData.append('name', author.name)
    formData.append('surname', author.surname)
    formData.append('bio', author.bio)
    formData.append('profilePicture', author.profilePicture)
    formData.append('id', id)
    return await axios.patch(`/authors/${id}`, formData)
  }

  public static async getAuthor(id: string) {
    return await axios.get(`/authors/${id}`)
  }
}
