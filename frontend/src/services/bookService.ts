import axios from 'axios'
import { Book, GetBooksRequest } from '../models/entry/book'
import { Entry } from '../models/entry/Entry'

export class BookService {
  public static async getBook(id: string) {
    return await axios.get<Book>('book', {
      params: {
        id,
      },
    })
  }

  public static async getBooks(params: GetBooksRequest) {
    return await axios.get<{ books: Entry[] }>('book/page', { params })
  }
}
