import { AmaSession, AmaSessionFormData } from '../models/amaSessions'
import axios from 'axios'

export class amaSessionService {
  public static async postAmaSession(amaSession : AmaSessionFormData){
    return await axios.post<AmaSession>('/ama-sessions', amaSession);
  }
}