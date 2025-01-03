import { Article, ArticleFormData } from '../models/article'
import axios from 'axios'

export class articleService {
  public static async postArticle(article: ArticleFormData) {
    return await axios.post<Article>('articles', article)
  }

  public static async putArticle(article: ArticleFormData) {
    return await axios.put(`/articles/${article.id}`, article)
  }
  public static async getArticle(id: string) {
    return await axios.get<Article>(`/articles/${id}`)
  }
}
