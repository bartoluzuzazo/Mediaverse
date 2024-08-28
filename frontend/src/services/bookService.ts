import axios from 'axios'

export class bookService {
  public static async getBook(id: string) {
    return await axios.get('book', {
      params: {
        'id': id,
      },
    })
  }
}