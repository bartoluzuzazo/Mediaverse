import { OrderDirection, PaginateRequest } from '../../common'
import { Entry, EntryOrder } from '../Entry.ts'

export interface Book {
  entry: Entry
  isbn: string
  synopsis: string
  bookGenres: string[]
}

export interface GetEntryPageRequest extends PaginateRequest {
  order: EntryOrder
  direction: OrderDirection
}
