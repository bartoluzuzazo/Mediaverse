import { FunctionComponent } from 'react'
import { useNavigate } from '@tanstack/react-router'
import { EpisodePreview } from '../../../../models/entry/series/episode/Episode.ts'

interface Props {
  episode: EpisodePreview
  url? : string
}

const SeriesEpisodePreview: FunctionComponent<Props> = ({ episode, url }) => {
  const navigate = useNavigate()
  const handleLink = async () => {
    await navigate({ to: url ? url : `/entries/${episode.id}` })
  }
  return (
    <>
      <div
        className="flex cursor-pointer flex-row justify-between"
        onClick={handleLink}
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
