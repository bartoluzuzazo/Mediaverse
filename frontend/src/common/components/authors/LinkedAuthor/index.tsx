import { FunctionComponent } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { authorService } from '../../../../services/authorService.ts'
import { serviceUtils } from '../../../../services/serviceUtils.ts'
import { FaLink, FaUnlink } from 'react-icons/fa'
import { UserSearch } from '../../users/UserSearch'
import { User } from '../../../../models/user'
import CustomImage from '../../customImage'
import { Link } from '@tanstack/react-router'
import { userService } from '../../../../services/userService.ts'
import { ModalButton } from '../../shared/ModalButton'

type Props = {
  authorId: string
}

export const LinkedUser: FunctionComponent<Props> = ({ authorId }) => {
  const { data: linkedUser, isLoading } = useQuery({
    queryKey: ['GET_LINKED_AUTHOR', authorId],
    queryFn: async () => {
      const getFn = () => authorService.getLinkedUser(authorId)
      return await serviceUtils.getIfFound(getFn)
    },
  })
  if (isLoading) {
    return null
  }
  if (!linkedUser) {
    return <LinkUserComponent authorId={authorId} />
  }
  return <UnlinkUserComponent linkedUser={linkedUser} authorId={authorId} />
}
type UnlinkUserProps = {
  linkedUser: User
  authorId: string
}

const UnlinkUserComponent: FunctionComponent<UnlinkUserProps> = ({
  linkedUser,
  authorId,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync: unlinkUserMutation } = useMutation({
    mutationFn: async () => await authorService.deleteLinkedUser(authorId),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['GET_LINKED_AUTHOR', authorId],
      })
    },
  })
  return (
    <div className="mt-2 flex w-full items-center gap-3 rounded-md bg-violet-200 p-1">
      <Link to="/users/$id" params={{ id: linkedUser.id }}>
        <CustomImage
          className="aspect-square w-10 self-center rounded-full object-cover shadow-lg"
          src={`data:image/webp;base64,${linkedUser.profilePicture}`}
        />
      </Link>
      <span className="text-lg font-semibold">{linkedUser.username}</span>
      <button
        type="button"
        onClick={async () => {
          await unlinkUserMutation()
        }}
        className="m-1 ml-auto grid aspect-square w-10 place-content-center self-center rounded-full bg-violet-900 p-0 text-white"
      >
        <FaUnlink className="text-lg" />
      </button>
    </div>
  )
}

const LinkUserComponent: FunctionComponent<Props> = ({ authorId }) => {
  const queryClient = useQueryClient()
  const { mutateAsync: linkUserMutation } = useMutation({
    mutationFn: async (userId: string) =>
      await authorService.addLinkedUser(authorId, userId),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['GET_LINKED_AUTHOR', authorId],
      })
    },
  })

  return (
    <ModalButton icon={<FaLink className="text-xl"/>}  text={"Link Author"}>
      <UserSearch
        searchFunction={userService.search}
        onClick={async (u) => {
          await linkUserMutation(u.id)
        }}
        queryKey="SEARCH_USER"
      />
    </ModalButton>
  )
}