import axios from 'axios'

import { Entry } from '../models/entry/Entry'
import { Book, GetEntryPageRequest } from '../models/entry/book'
import { BookFormData } from '../common/components/entries/books/BookForm.tsx'

export class BookService {
  public static async getBook(id: string) {
    return await axios.get<Book>(`/book/${id}`)
  }

  public static async getBooks(params: GetEntryPageRequest) {
    return await axios.get<{ books: Entry[] }>('book/page', { params })
  }

  public static async postBook(book: BookFormData) {
    return await axios.post('/book', book)
  }


  public static async patchBook(book: BookFormData, id: string) {
    return await axios.patch(`/book/${id}`, book)
  }
}

