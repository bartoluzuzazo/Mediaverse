import { FunctionComponent } from 'react'
import { Entry } from '../../../models/entry/Entry'
import { Link } from '@tanstack/react-router'
import CustomImage from '../customImage'
import { CustomCarousel } from '../shared/CustomCarousel'

interface EntryCarouselProps {
  entries: Entry[]
  title: string
}

const EntryCarousel: FunctionComponent<EntryCarouselProps> = ({
  entries,
  title,
}) => {
  if (entries.length <= 0) {
    return <></>
  }

  return (
    <div>
      <p className="select-none text-2xl font-bold">{title}</p>
      <CustomCarousel>
        {entries.map(({ id, photo, name}) => (
          <Link
            key={id}
            className="m-1 my-3 block h-[235px] w-[170px] overflow-hidden transition-shadow"
            to="/entries/$id"
            params={{ id }}
          >
            <div className="absolute top-1/2 p-3 text-center center text-white bg-black w-[170px] h-1/4 bg-opacity-30 align-middle pointer-events-none">
              {name}
            </div>
            <CustomImage
              className="h-full w-full object-cover transition-all hover:scale-[1.1]"
              src={`data:image/webp;base64,${photo}`}
            />
          </Link>
        ))}
      </CustomCarousel>
    </div>
  )
}

export default EntryCarousel
