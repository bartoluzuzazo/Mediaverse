import { FunctionComponent, MouseEventHandler } from 'react'
import { EpisodePreview } from '../../../../models/entry/episode/Episode.ts'

interface Props {
  episode: EpisodePreview
  onClick: MouseEventHandler<HTMLDivElement>
}

const SeriesEpisodePreview: FunctionComponent<Props> = ({ episode, onClick }) => {
  return (
    <>
      <div
        className="flex cursor-pointer flex-row justify-between"
        onClick={onClick}
      >
        <div className="flex flex-col p-2">
          <h2 className="p-2">{`Episode ${episode.episodeNumber}`}</h2>
        </div>
        <div className="flex flex-col p-2">
          <h2 className="p-2">{episode.name}</h2>
        </div>
        <div className="flex flex-col p-2">
          <h2 className="p-2">{episode.release.toString()}</h2>
        </div>
        <div className="flex flex-col p-2">
          <h2 className="p-2">{episode.ratingAvg.toFixed(2)}★</h2>
        </div>
      </div>
    </>
)
}

export default SeriesEpisodePreview
