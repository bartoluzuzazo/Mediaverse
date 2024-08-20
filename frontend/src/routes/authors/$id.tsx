import { createFileRoute } from '@tanstack/react-router'
import { AuthorService } from '../../services/AuthorService.ts'
import { Author } from '../../models/author/Author.ts'

export const Route = createFileRoute('/authors/$id')({
  loader: async ({ params }) => {
    const response = await AuthorService.getAuthor(params.id)
    return response.data
  },
  component: () => {
    const author = Route.useLoaderData<Author>()
    const imgSrc = 'data:image/*;base64,' + author.base64Picture
    return (
      <>
        <div className="-mx-[calc(50vw-50%)] h-20 bg-violet-800"></div>

        <div className="flex flex-col p-4 md:flex-row">
          <div>
            <img
              src={imgSrc}
              className="-mt-16 aspect-square w-52 rounded-full border-4 border-white bg-slate-300 object-cover md:w-60"
              alt="profile picture"
            />
            <div className="text-xl font-bold">
              {author.name}{' '}
              <span className="font-italic">{author.surname}</span>
            </div>
          </div>

          <div className="flex-1 md:ml-20">{author.bio}</div>
        </div>
      </>
    )
  },
})
