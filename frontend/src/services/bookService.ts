import axios from 'axios'
import { Book } from '../models/entry/book/Book.ts'

export class bookService {
  public static async getBook(id: string) {
    return await axios.get<Book>('book', {
      params: {
        id: id,
      },
    })
  }
}
