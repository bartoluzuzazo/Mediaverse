import { FunctionComponent, ReactNode } from 'react'

import { AxiosResponse } from 'axios'
import { useMutation, useQueryClient } from '@tanstack/react-query'

type Props = {
  icon: ReactNode
  text: string
  isSecondary?: boolean
  friendId: string
  mutationFn: (friendId: string) => Promise<AxiosResponse<unknown, unknown>>
}
export const FriendshipButtonBase: FunctionComponent<Props> = ({
  icon,
  text,
  isSecondary,
  friendId,
  mutationFn,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync } = useMutation({
    mutationFn,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['GET_FRIENDSHIP', friendId] })
      queryClient.invalidateQueries({ queryKey: ['GET_FRIENDS', friendId] })
    },
  })
  const onClick = async () => {
    await mutateAsync(friendId)
  }
  return (
    <button
      onClick={onClick}
      type="button"
      className={`mt-2 flex w-full items-center gap-3 px-4 py-2 font-semibold ${
        isSecondary ? `bg-slate-300 text-slate-950` : `bg-slate-950 text-white`
      }`}
    >
      {icon}
      <span>{text}</span>
    </button>
  )
}
