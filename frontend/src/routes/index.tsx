import { createFileRoute } from '@tanstack/react-router'
import { BookService } from '../services/EntryServices/bookService.ts'
import { EntryOrder } from '../models/entry/Entry'
import { OrderDirection } from '../models/common'
import { Oval } from 'react-loader-spinner'
import { MovieService } from '../services/EntryServices/movieService.ts'
import { GameService } from '../services/EntryServices/gameService.ts'
import { SeriesService } from '../services/EntryServices/seriesService.ts'
import { SongService } from '../services/EntryServices/songService.ts'
import { AlbumService } from '../services/EntryServices/albumService.ts'
import { EpisodeService } from '../services/EntryServices/episodeService.ts'

export const Route = createFileRoute('/')({
  pendingComponent: () => (
    <div className="pointer-events-none fixed bottom-0 left-0 right-0 top-0 flex items-center justify-center">
      <Oval
        visible={true}
        height="80"
        width="80"
        color="#5b21b6"
        secondaryColor=""
        ariaLabel="oval-loading"
        wrapperStyle={{}}
        wrapperClass=""
      />
    </div>
  ),
  loader: async ({ context: { queryClient } }) => {
    const topParams = {
      page: 1,
      size: 10,
      order: EntryOrder.Rating,
      direction: OrderDirection.Descending,
    }
    const newestParams = {
      page: 1,
      size: 10,
      order: EntryOrder.Release,
      direction: OrderDirection.Descending,
    }
    const getPageFunctions = [
      { function: BookService.getBooks, entryType: 'Books' },
      { function: MovieService.getMovies, entryType: 'Movies' },
      { function: GameService.getGames, entryType: 'Games' },
      { function: SeriesService.getSeriesPage, entryType: 'Series' },
      { function: EpisodeService.getEpisodes, entryType: 'Episodes' },
      { function: SongService.getSongs, entryType: 'Songs' },
      { function: AlbumService.getAlbums, entryType: 'Albums' },
    ]
    const data = []
    for (const item of getPageFunctions) {
      const top = await queryClient.ensureQueryData({
        queryKey: ['top', item.entryType.toLowerCase()],
        queryFn: async () =>
          await item.function(topParams),
      })
      data.push({data: top, title: `Top ${item.entryType}`})
      const newest = await queryClient.ensureQueryData({
        queryKey: ['newest', item.entryType.toLowerCase()],
        queryFn: async () =>
          await item.function(newestParams),
      })
      data.push({data: newest, title: `Newest ${item.entryType}`})
    }
    return data
  },
})
