import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute } from '@tanstack/react-router'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../../../common/components/entries/entryBanner.tsx'
import EntryRatingPicker from '../../../common/components/entryRatingPicker'
import { Book } from '../../../models/entry/book'
import { BookService } from '../../../services/bookService.ts'
import EntryAuthorPreview from '../../../common/components/entries/entryAuthorPreview.tsx'
import SectionHeader from '../../../common/components/entries/sectionHeader.tsx'
import CommentSection from '../../../common/components/comments/CommentSection.tsx'
import { AuthorizedView } from '../../../common/components/auth/AuthorizedView'
import { LinkButton } from '../../../common/components/shared/LinkButton'
import { FaPen } from 'react-icons/fa'

interface BookEntryComponentProps {}

const bookQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_BOOK', id],
    queryFn: async (): Promise<Book> => {
      const res = await BookService.getBook(id)
      return res.data
    },
  })
}

const BookEntryComponent: FunctionComponent<BookEntryComponentProps> = () => {
  const id = Route.useParams().id
  const bookQuery = useSuspenseQuery(bookQueryOptions(id))
  const book = bookQuery.data
  const info = [book.entry.release.toString(), book.isbn, ...book.bookGenres]

  return (
    <>
      <EntryBanner entry={book.entry} info={info} type={'Book'} />
      <AuthorizedView allowedRoles="Administrator">
        <div className='max-w-32 mt-4 -mb-2'>
          <LinkButton to={'/entries/books/edit/$id'} params={{id: book.entry.id}} icon={<FaPen/>}>Edit</LinkButton>
        </div>
      </AuthorizedView>
      <AuthorizedView>
        <EntryRatingPicker entryId={book.entry.id} />
      </AuthorizedView>
      <SectionHeader title={'Description'} />
      <div className="p-4">{book.entry.description}</div>
      {book.entry.authors.map((group) => (
        <Fragment key={group.role}>
          <SectionHeader title={group.role} />
          <div className="flex flex-wrap">
            {group.authors.map((a) => (
              <div className="p-2" key={a.id}>
                <EntryAuthorPreview author={a} />
              </div>
            ))}
          </div>
        </Fragment>
      ))}
      <SectionHeader title={'Synopsis'} />
      <div className="p-4">{book.synopsis}</div>
      <CommentSection entryId={id} />
    </>
  )
}

export const Route = createFileRoute('/entries/books/$id')({
  loader: async ({ context: { queryClient }, params: { id } }) => {
    return queryClient.ensureQueryData(bookQueryOptions(id))
  },

  component: BookEntryComponent,
})
