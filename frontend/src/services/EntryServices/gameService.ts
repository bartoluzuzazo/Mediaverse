import axios from 'axios'

import { Entry } from '../../models/entry/Entry.ts'
import { Game } from '../../models/entry/game/Game.ts'
import { GameFormData } from '../../common/components/entries/games/GameForm.tsx'
import { GetEntryPageRequest } from '../../models/entry/book'

export class GameService {
  public static async getGame(id: string) {
    return await axios.get<Game>(`/game/${id}`)
  }

  public static async getGames(params: GetEntryPageRequest) {
    return await axios.get<{ entries: Entry[] }>('game/page', { params })
  }

  public static async postGame(game: GameFormData) {
    return await axios.post('/game', game)
  }

  public static async patchGame(game: GameFormData, id: string) {
    return await axios.patch(`/game/${id}`, game)
  }
}

