import { Comment } from '../../../models/comments'

type Props = {
  comment: Comment
}
export const CommentView = ({ comment }: Props) => {
  console.log(comment.username)
  const imgSrc = 'data:image/*;base64,' + comment.userProfile
  return (
    <div className="mb-4 flex gap-6 rounded-xl p-3 shadow-md">
      <div>
        <img
          src={imgSrc}
          alt="users profile"
          className="picture aspect-square w-24 rounded-full border-2 object-cover shadow-md"
        />
        <div className="text-center">{comment.username}</div>
      </div>
      <div className="flex-1">{comment.content}</div>
    </div>
  )
}
