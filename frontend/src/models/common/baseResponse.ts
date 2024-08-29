export interface BaseResponse<T = void> {
  data: T
  exception: { message: string }
}
