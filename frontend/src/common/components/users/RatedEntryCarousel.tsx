import SectionHeader from '../entries/sectionHeader.tsx'
import { useQuery } from '@tanstack/react-query'
import { FunctionComponent } from 'react'
import { userService } from '../../../services/userService.ts'
import { OrderDirection } from '../../../models/common'
import { RatedEntryOrder } from '../../../models/entry/ratedEntry'
import { CustomCarousel } from '../shared/CustomCarousel'
import { Link } from '@tanstack/react-router'
import CustomImage from '../customImage'
import { AiFillStar } from 'react-icons/ai'

type Props = {
  userId: string
}
export const RatedEntryCarousel: FunctionComponent<Props> = ({ userId }) => {
  const { data: entries } = useQuery({
    queryKey: ['GET_RATED_ENTRIES', userId],
    queryFn: async () => {
      return (
        await userService.getRatedEntries(userId, {
          page: 1,
          size: 20,
          direction: OrderDirection.Descending,
          order: RatedEntryOrder.RatedByUserAt,
        })
      ).data
    },
  })
  if (!entries || entries.contents.length === 0) {
    return null
  }
  return (
    <div>
      <SectionHeader title="Recently rated entries" />
      <CustomCarousel>
        {entries.contents.map((entry) => {
          return (
            <div key={entry.id}>
              <Link
                className="m-1 my-3 block h-[235px] w-[170px] overflow-hidden transition-shadow"
                to="/entries/books/$id"
                params={{ id: entry.id }}
              >
                <CustomImage
                  className="h-full w-full object-cover transition-all hover:scale-[1.1]"
                  src={`data:image/webp;base64,${entry.photo}`}
                />
              </Link>
              <div className="flex items-center gap-0.5 font-semibold text-black">
                Rated: {entry.usersRating} <AiFillStar className="mt-0.5" />
              </div>
            </div>
          )
        })}
      </CustomCarousel>
    </div>
  )
}
