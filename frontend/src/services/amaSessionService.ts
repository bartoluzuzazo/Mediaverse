import {
  AmaQuestion,
  AmaQuestionAnswerFormData,
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

  public static async putAmaSessionAnswer(
    questionId: string,
    answer: AmaQuestionAnswerFormData
  ) {
    return await axios.put(
      `/ama-sessions/questions/${questionId}/answer`,
      answer
    )
  }

  public static async putQuestionLike(questionId: string) {
    return await axios.put(`/ama-sessions/questions/${questionId}/like`)
  }
  public static async deleteQuestionLike(questionId: string) {
    return await axios.delete(`/ama-sessions/questions/${questionId}/like`)
  }

  public static async endSession(sessionId: string) {
    return await axios.put(`/ama-sessions/${sessionId}/ending`)
  }
}
