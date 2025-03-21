import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { Fragment, FunctionComponent } from 'react'
import EntryBanner from '../entryBanner.tsx'
import EntryRatingPicker from '../../entryRatingPicker'
import { Book } from '../../../../models/entry/book'
import { BookService } from '../../../../services/EntryServices/bookService.ts'
import EntryAuthorPreview from '../AuthorPreview.tsx'
import SectionHeader from '../sectionHeader.tsx'
import CommentSection from '../../comments/CommentSection.tsx'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { LinkButton } from '../../shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { ReviewsCarousel } from '../../reviews/reviewsCarousel'

interface BookEntryComponentProps {
  id: string
}

const bookQueryOptions = (id: string) => {
  return queryOptions({
    queryKey: ['GET_ENTRY', id],
    queryFn: async (): Promise<Book> => {
      const res = await BookService.getBook(id)
      return res.data
    },
  })
}

export const BookEntryComponent: FunctionComponent<BookEntryComponentProps> = ({
  id,
}) => {
  const bookQuery = useSuspenseQuery(bookQueryOptions(id))
  const book = bookQuery.data
  const info = [book.entry.release.toString(), book.isbn, ...book.bookGenres]

  return (
    <>
      <EntryBanner entry={book.entry} info={info} type={'Book'} />
      <AuthorizedView allowedRoles="ContentCreator">
        <div className="-mb-2 mt-4 max-w-32">
          <LinkButton
            to={'/entries/books/edit/$id'}
            params={{ id: book.entry.id }}
            icon={<FaPen />}
          >
            Edit
          </LinkButton>
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
      <ReviewsCarousel entryId={id} />
      <CommentSection entryId={id} />
    </>
  )
}
