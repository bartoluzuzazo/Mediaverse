import { createFileRoute } from '@tanstack/react-router'
import { authorService } from '../../services/authorService.ts'
import { Author } from '../../models/author/Author.ts'
import AuthorEntryPreview from '../../common/components/authors/Entry/AuthorEntryPreview.tsx'
import SectionHeader from '../../common/components/entries/sectionHeader.tsx'
import { AuthorizedView } from '../../common/components/auth/AuthorizedView'
import { LinkedUser } from '../../common/components/authors/LinkedAuthor'
import { LinkButton } from '../../common/components/shared/LinkButton'
import { FaPen } from 'react-icons/fa'
import { CreateAmaComponent } from '../../common/components/ama/createAmaComponent'
import { Fragment } from 'react'
import { ToggledView } from '../../common/components/shared/ToggledView'
import { AuthorsAmasComponent } from '../../common/components/ama/authorsAmasComponent'

export const Route = createFileRoute('/authors/$id')({
  loader: async ({ params }) => {
    const response = await authorService.getAuthor(params.id)
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
            <AuthorsAmasComponent authorId={author.id} />
            <AuthorizedView allowedRoles="Administrator">
              <LinkedUser authorId={author.id} />
            </AuthorizedView>
            <AuthorizedView requiredUserId={author.userId}>
              <ToggledView containerClass="min-w-fit" title="Manage">
                <LinkButton
                  to={`/authors/edit/$id`}
                  params={{ id: author.id }}
                  icon={<FaPen />}
                >
                  Edit author
                </LinkButton>
                <CreateAmaComponent authorId={author.id} />
              </ToggledView>
            </AuthorizedView>
          </div>

          <div className="flex-1 md:ml-20">{author.bio}</div>
        </div>
        {author.workOns.map((group) => (
          <Fragment key={group.role}>
            <SectionHeader title={group.role} />
            {group.entries.map((e) => (
              <div className="p-2" key={e.id}>
                <AuthorEntryPreview entry={e} />
              </div>
            ))}
          </Fragment>
        ))}
      </>
    )
  },
})
