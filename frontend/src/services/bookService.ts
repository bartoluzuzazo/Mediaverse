import axios from 'axios'
<<<<<<< HEAD
import { GetBooksRequest } from '../models/entry/book'
import { Entry } from '../models/entry/Entry'
=======
import { Book } from '../models/entry/book/Book.ts'
>>>>>>> origin/main

export class BookService {
  public static async getBook(id: string) {
    return await axios.get<Book>('book', {
      params: {
<<<<<<< HEAD
        id,
      },
    })
  }

  public static async getBooks(params: GetBooksRequest) {
    return await axios.get<{ books: Entry[] }>('book/page', { params })
  }
=======
        id: id,
      },
    })
  }
>>>>>>> origin/main
}

        id,

  public static async getBooks(params: GetBooksRequest) {
    return await axios.get<{ books: Entry[] }>('book/page', { params })
  }
