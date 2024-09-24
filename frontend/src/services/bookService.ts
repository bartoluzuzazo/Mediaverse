import axios from 'axios'
import { BookFormData } from '../common/components/entries/books/BookForm.tsx'
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

  public static async postBook(book: BookFormData) {
    return await axios.post('/book', book)
  }

  public static async patchBook(book: BookFormData, id: string) {
    return await axios.patch(`/book/${id}`, book, {
      params: {
        'id': id,
      },
    })
  }
}