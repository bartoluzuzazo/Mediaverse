export interface BaseResponse<T = void> {
  data: T
  exception: { message: string }
}

export interface PaginateRequest {
  page: number
  size: number
}

export enum OrderDirection {
  Descending,
  Ascending,
}

export interface Page<T> {
  contents: T[]
  pageCount: number
  currentPage: number
  hasNext: boolean
}
