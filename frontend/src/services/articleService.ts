import {
  Article,
  ArticleFormData,
  ArticlePreview,
  SearchArticleParams,
} from '../models/article'
import axios from 'axios'
import { Page } from '../models/common'

export class articleService {
  public static async getArticles() {
    return await axios.get<Article[]>('articles')
  }

  public static async postArticle(article: ArticleFormData) {
    return await axios.post<Article>('articles', article)
  }

  public static async putArticle(article: ArticleFormData) {
    return await axios.put(`/articles/${article.id}`, article)
  }
  public static async getArticle(id: string) {
    return await axios.get<Article>(`/articles/${id}`)
  }

  public static async searchArticles(params: SearchArticleParams) {
    return await axios.get<Page<ArticlePreview>>(`/articles/search`, { params })
  }
}
