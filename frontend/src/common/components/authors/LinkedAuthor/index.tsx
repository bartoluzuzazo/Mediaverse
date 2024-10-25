import { FunctionComponent, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { AuthorService } from '../../../../services/AuthorService.ts'
import { serviceUtils } from '../../../../services/serviceUtils.ts'
import { FaLink, FaUnlink } from 'react-icons/fa'
import { Modal } from '../../shared/Modal'
import { UserSearch } from '../../users/UserSearch'
import { User } from '../../../../models/user'
import CustomImage from '../../customImage'
import { Link } from '@tanstack/react-router'

type Props = {
  authorId: string
}

export const LinkedUser: FunctionComponent<Props> = ({ authorId }) => {
  const { data: linkedUser, isLoading } = useQuery({
    queryKey: ['GET_LINKED_AUTHOR', authorId],
    queryFn: async () => {
      const getFn = () => AuthorService.getLinkedUser(authorId)
      return await serviceUtils.getIfFound(getFn)
    },
  })
  if (isLoading) {
    return null
  }
  if (!linkedUser) {
    return (
      <LinkUserComponent authorId={authorId} />
    )
  }
  return (
    <UnlinkUserComponent linkedUser={linkedUser} authorId={authorId}/>
  )
}
type UnlinkUserProps={
  linkedUser: User,
  authorId: string
}

const UnlinkUserComponent: FunctionComponent<UnlinkUserProps> =({linkedUser, authorId})=>{
  const queryClient = useQueryClient()
  const { mutateAsync: unlinkUserMutation } = useMutation({
    mutationFn: async ()=> await AuthorService.deleteLinkedUser(authorId),
    onSuccess: async ()=>{
      await queryClient.invalidateQueries({
        queryKey: ['GET_LINKED_AUTHOR', authorId]
      })
    }
  })
  return(
    <div className='mt-2 bg-violet-200 flex items-center rounded-md gap-3 p-1 w-full'>
      <Link
        to="/users/$id"
        params={{ id: linkedUser.id }}
      >
      <CustomImage
        className='aspect-square w-10 self-center rounded-full object-cover shadow-lg'
        src={`data:image/webp;base64,${linkedUser.profilePicture}`}
      />
      </Link>
      <span className='font-semibold text-lg'>{linkedUser.username}</span>
      <button type="button" onClick={async ()=>{ await unlinkUserMutation()}}
        className='self-center ml-auto text-white bg-violet-900 rounded-full w-10 aspect-square m-1 grid place-content-center p-0 '>
        <FaUnlink className='text-lg' />
      </button>
    </div>
  )
}


const LinkUserComponent: FunctionComponent<Props> = ({ authorId }) => {
  const [isOpen, setIsOpen] = useState<boolean>(false)
  const queryClient = useQueryClient()
  const { mutateAsync: linkUserMutation } = useMutation({
    mutationFn: async (userId: string)=> await AuthorService.addLinkedUser(authorId, userId ),
    onSuccess: async ()=>{
      await queryClient.invalidateQueries({
        queryKey: ['GET_LINKED_AUTHOR', authorId]
      })
    }
  })

  return (
    <>
      <button className='mt-2 bg-violet-200 flex items-center rounded-md gap-3 p-1 w-full border-none hover:scale-105' onClick={() => setIsOpen(true)}>
        <div
          className='self-center text-white bg-violet-900 rounded-full w-10 aspect-square m-1 grid place-content-center'>
          <FaLink className='text-xl' />
        </div>
        <span className='font-semibold text-lg'>Link Author</span>
      </button>
      {isOpen &&
        <Modal onOutsideClick={() => setIsOpen(false) }>
          <UserSearch  onClick={async (u)=>{ await linkUserMutation(u.id)}}/>
        </Modal>}
    </>
  )
}