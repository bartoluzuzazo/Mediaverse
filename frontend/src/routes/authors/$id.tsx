import { createFileRoute } from '@tanstack/react-router'
import { AuthorService } from '../../services/AuthorService.ts'
import { Author } from '../../models/author/Author.ts'
import AuthorEntryPreview from '../../common/components/authors/Entry/AuthorEntryPreview.tsx'
import SectionHeader from '../../common/components/entries/sectionHeader.tsx'
import { AuthorizedView } from '../../common/components/auth/AuthorizedView'
import { LinkedUser } from '../../common/components/authors/LinkedAuthor'
import { LinkButton } from '../../common/components/shared/LinkButton'
import { FaPen } from 'react-icons/fa'

export const Route = createFileRoute('/authors/$id')({
  loader: async ({ params }) => {
    const response = await AuthorService.getAuthor(params.id)
    return response.data
  },
  component: () => {
    const author = Route.useLoaderData<Author>()
    const imgSrc = 'data:image/*;base64,' + author.profilePicture
    return (
      <>
        <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800 md:h-32"></div>

        <div className="flex flex-col p-4 md:flex-row">
          <div>
            <img
              src={imgSrc}
              className="-mt-16 aspect-square w-52 rounded-full border-4 border-white bg-slate-300 object-cover md:-mt-24 md:w-60"
              alt="profile picture"
            />
            <div className="text-xl font-bold">
              {author.name}{' '}
              <span className="font-italic">{author.surname}</span>
            </div>
            <AuthorizedView allowedRoles='Administrator'>
              <LinkedUser authorId={author.id} />
            </AuthorizedView>
            <LinkButton to={`/authors/edit/$id`} params={{id: author.id}} icon={<FaPen/>}>Edit author</LinkButton>
          </div>

          <div className="flex-1 md:ml-20">{author.bio}</div>
        </div>
        {author.workOns.map((group) => (
          <>
            <SectionHeader title={group.role} />
            {group.entries.map((e) => (
              <div className="p-2">
                <AuthorEntryPreview entry={e} />
              </div>
            ))}
          </>
        ))}
      </>
    )
  },
})
