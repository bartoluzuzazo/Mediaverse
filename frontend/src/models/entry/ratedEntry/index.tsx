import { OrderDirection, PaginateRequest } from '../../common'

export interface RatedEntry {
  id: string
  name: string
  photo: string
  usersRating: number
  type: string
}

export enum RatedEntryOrder {
  RatedByUserAt,
}
export interface GetRatedEntryPageRequest extends PaginateRequest {
  order: RatedEntryOrder
  direction: OrderDirection
}
