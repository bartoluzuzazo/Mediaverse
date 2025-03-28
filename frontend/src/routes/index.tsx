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
import { articleService } from '../services/articleService.ts'
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
    const promises = []
    for (const item of getPageFunctions) {
      const top = queryClient
        .ensureQueryData({
          queryKey: ['top', item.entryType.toLowerCase()],
          queryFn: async () => await item.function(topParams),
        })
        .then((res) => {
          return { data: res, title: `Top ${item.entryType}` }
        })
      promises.push(top)
      const newest = queryClient
        .ensureQueryData({
          queryKey: ['newest', item.entryType.toLowerCase()],
          queryFn: async () => await item.function(newestParams),
        })
        .then((res) => {
          return { data: res, title: `Newest ${item.entryType}` }
        })
      promises.push(newest)
    }
    const dataPromise = Promise.allSettled(promises).then((results) => {
      return results
        .filter((res) => res.status === 'fulfilled')
        .map((res) => res.value)
    })

    const articlesPromise = queryClient.ensureQueryData({
      queryKey: ['newest', 'articles'],
      queryFn: articleService.getArticles,
    })

    const loadedData = await Promise.all([dataPromise, articlesPromise]).then(
      (results) => {
        return { data: results[0], articles: results[1].data }
      }
    )

    return loadedData
  },
})
