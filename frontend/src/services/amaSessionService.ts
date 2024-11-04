import { AmaSession, AmaSessionFormData } from '../models/amaSessions'
import axios from 'axios'

export class amaSessionService {
  public static async postAmaSession(amaSession : AmaSessionFormData){
    return await axios.post<AmaSession>('/ama-sessions', amaSession);
  }
  public static async getAmaSession(id: string){
    return await axios.get<AmaSession>(`/ama-sessions/${id}`)
  }
}