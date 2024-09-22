import axios, { AxiosResponse } from 'axios'

export class serviceUtils {
  public static async getIfFound<T>(
    getFn: () => Promise<AxiosResponse<T, unknown>>
  ): Promise<T | null> {
    try {
      const response = await getFn()
      return response.data
    } catch (error: unknown) {
      if (!axios.isAxiosError(error) || error.response?.status !== 404) {
        throw error
      }
      return null
    }
  }
}
