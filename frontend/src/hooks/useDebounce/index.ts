import { useEffect } from 'react'

export const useDebounce = (
  handler: () => void | Promise<void>,
  timeout: number,
) => {
  useEffect(() => {
    const intervalId = setTimeout(handler, timeout)
    return () => clearTimeout(intervalId)
  }, [handler, timeout])
}
