import { FunctionComponent } from 'react'
import { ModalButton } from '../../shared/ModalButton'
import { FaQuestion } from 'react-icons/fa'
import { useQuery } from '@tanstack/react-query'
import { amaSessionService } from '../../../../services/amaSessionService.ts'
import { AmaSessionTile } from './amaSessionTile.tsx'

interface AuthorsAmasComponentProps {
  authorId: string
}

export const AuthorsAmasComponent: FunctionComponent<
  AuthorsAmasComponentProps
> = ({ authorId }) => {
  return (
    <ModalButton icon={<FaQuestion />} text="View AMA sessions">
      <AuthorsAmasView authorId={authorId} />
    </ModalButton>
  )
}

interface AuthorsAmasViewProps {
  authorId: string
}

const AuthorsAmasView: FunctionComponent<AuthorsAmasViewProps> = ({
  authorId,
}) => {
  const { data: amaSessions, isLoading } = useQuery({
    queryKey: ['GET_AUTHORS_SESSIONS', authorId],
    queryFn: async () => {
      const response = await amaSessionService.getAuthorsAmaSessions(authorId)
      return response.data
    },
  })

  if (isLoading) {
    return <p>Loading...</p>
  }
  return (
    <div className="space-y-6">
      <h2 className="capitalize">Ama sessions</h2>
      {amaSessions!.map((s) => (
        <AmaSessionTile amaSession={s} key={s.id} />
      ))}
    </div>
  )
}
