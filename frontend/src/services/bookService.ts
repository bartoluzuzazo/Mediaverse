import axios from 'axios'
import { BookFormData } from '../common/components/entries/books/bookForm.tsx'
import { Book, GetEntryPageRequest } from '../models/entry/book'
import { Entry } from '../models/entry/Entry'

export class BookService {
  public static async getBook(id: string) {
    return await axios.get<Book>('book', {
      params: {
        id,
      },
    })
  }

  public static async getBooks(params: GetEntryPageRequest) {
    return await axios.get<{ books: Entry[] }>('book/page', { params })
  }

  public static async postBook(author: BookFormData) {
    return await axios.post('/book', author)
  }

  public static async patchBook(author: BookFormData, id: string) {
    return await axios.patch(`/book/${id}`, author, {
      params: {
        id: id,
      },
    })
  }
}
