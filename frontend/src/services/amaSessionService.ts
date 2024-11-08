import {
  AmaQuestion,
  AmaQuestionFormData,
  AmaSession,
  AmaSessionFormData,
  GetAmaQuestionParams,
} from '../models/amaSessions'
import axios from 'axios'
import { Page } from '../models/common'

export class amaSessionService {
  public static async postAmaSession(amaSession: AmaSessionFormData) {
    return await axios.post<AmaSession>('/ama-sessions', amaSession)
  }

  public static async getAmaSession(id: string) {
    return await axios.get<AmaSession>(`/ama-sessions/${id}`)
  }

  public static async postAmaQuestion(
    sessionId: string,
    question: AmaQuestionFormData
  ) {
    return await axios.post(`/ama-sessions/${sessionId}/questions`, question)
  }

  public static async getAmaQuestions(
    sessionId: string,
    params: GetAmaQuestionParams
  ) {
    return await axios.get<Page<AmaQuestion>>(
      `/ama-sessions/${sessionId}/questions`,
      { params }
    )
  }
  public static async getAmaQuestionsAuthorized(
    sessionId: string,
    params: GetAmaQuestionParams
  ) {
    return await axios.get<Page<AmaQuestion>>(
      `/ama-sessions/${sessionId}/authorized-questions`,
      { params }
    )
  }
}
