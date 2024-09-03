import { OrderDirection, PaginateRequest } from '../../common/index.ts'
import { Entry, EntryOrder } from '../Entry.ts'

export interface Book {
  entry: Entry
  isbn: string
  synopsis: string
  bookGenres: string[]
}

export interface GetBooksRequest extends PaginateRequest {
  order: EntryOrder
  direction: OrderDirection
}

