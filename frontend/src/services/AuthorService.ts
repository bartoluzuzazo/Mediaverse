import axios from 'axios'
import { AuthorFormData } from '../common/components/authors/AuthorForm.tsx'
import { User } from '../models/user'
import { Page, PaginateRequest } from '../models/common'
import { Author } from '../models/author/Author.ts'

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

  public static async getLinkedUser(authorId: string) {
    return await axios.get<User>(`/authors/${authorId}/linked-user`)
  }

  public static async addLinkedUser(authorId: string, userId: string) {
    return await axios.put(`/authors/${authorId}/linked-user`, { userId })
  }

  public static async deleteLinkedUser(authorId: string) {
    return await axios.delete(`/authors/${authorId}/linked-user`)
  }

  public static async search(query: string, params: PaginateRequest) {
    return await axios.get<Page<Author>>(`/authors/search`, { params: { ...params, query } })
  }
}
