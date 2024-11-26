import { FunctionComponent } from 'react'
import { AmaQuestion } from '../../../../models/amaSessions'
import {
  InfiniteData,
  useMutation,
  useQueryClient,
} from '@tanstack/react-query'
import { amaSessionService } from '../../../../services/amaSessionService.ts'
import { Page } from '../../../../models/common'
import { FaHeart, FaRegHeart } from 'react-icons/fa'

type Props = {
  question: AmaQuestion
  parentPage: number
  parentQueryKey: unknown[]
}

export const AmaQuestionLikes: FunctionComponent<Props> = ({
  question,
  parentQueryKey,
  parentPage,
}) => {
  const parentIndex = parentPage - 1
  const queryClient = useQueryClient()
  const { mutateAsync: changeLikeAsync } = useMutation({
    mutationFn: async (isLiked: boolean) => {
      if (isLiked) {
        return await amaSessionService.putQuestionLike(question.id)
      } else {
        return await amaSessionService.deleteQuestionLike(question.id)
      }
    },
    onMutate: (isLiked) => {
      const likeCountChange = isLiked ? 1 : -1

      const prevData =
        queryClient.getQueryData<InfiniteData<Page<AmaQuestion>>>(
          parentQueryKey
        )
      queryClient.setQueryData(
        parentQueryKey,
        (data: InfiniteData<Page<AmaQuestion>>) => {
          const copy = { ...data }
          copy.pages = [...data.pages]
          copy.pages[parentIndex] = { ...data.pages[parentIndex] }
          copy.pages[parentIndex].contents = data.pages[
            parentIndex
          ].contents.map((q) => {
            if (q.id !== question.id) {
              return q
            } else {
              return {
                ...q,
                likedByUser: isLiked,
                likes: q.likes + likeCountChange,
              }
            }
          })
          return copy
        }
      )

      return { prevData }
    },
    onError: (_err, _isLiked, context) => {
      queryClient.setQueryData(parentQueryKey, context!.prevData)
    },
  })
  return (
    <div className="flex items-center">
      <button
        className="gap-2 rounded-full border-none bg-transparent p-2 text-xl text-violet-700 transition-all hover:bg-slate-100 focus:outline-none"
        onClick={() => changeLikeAsync(!question.likedByUser)}
      >
        {question.likedByUser ? <FaHeart /> : <FaRegHeart />}
      </button>
      {question.likes}
    </div>
  )
}
