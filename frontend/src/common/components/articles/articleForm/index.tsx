import { FunctionComponent } from 'react'
import { Article, ArticleFormData } from '../../../../models/article'
import { SubmitHandler, useForm } from 'react-hook-form'
import FormTextArea from '../../entries/FormTextArea/FormTextArea.tsx'
import FormField from '../../form/FormField/FormField.tsx'
import { MarkdownField } from '../../form/markdownField'
import FormButton from '../../form/button'
import { articleService } from '../../../../services/articleService.ts'
import { useNavigate } from '@tanstack/react-router'

type Props = {
  article?: Article
}

export const ArticleForm: FunctionComponent<Props> = ({ article }) => {
  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ArticleFormData>({
    defaultValues: article
      ? {
          id: article.id,
          lede: article.lede,
          title: article.title,
          content: article.content,
        }
      : undefined,
  })
  const navigate = useNavigate()
  const onSubmit: SubmitHandler<ArticleFormData> = async (data) => {
    if (data.id === undefined) {
      const res = await articleService.postArticle(data)
      const id = res.data.id
      await navigate({ to: '/articles/$id', params: { id } })
    } else {
      await articleService.putArticle(data)
      await navigate({ to: '/articles/$id', params: { id: data.id } })
    }
  }
  return (
    <div className="p-4">
      <h1 className="my-4 text-2xl font-bold">New article</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormField<ArticleFormData>
          label="Title"
          registerPath="title"
          register={register}
          errorValue={errors.title}
          rules={{
            required: 'Title is required',
            maxLength: {
              value: 200,
              message: 'Maximum length is 200 characters',
            },
          }}
        />
        <FormTextArea<ArticleFormData>
          label="Lede"
          registerPath="lede"
          register={register}
          errorValue={errors.lede}
          rows={3}
          rules={{
            required: 'Lede is required',
            maxLength: {
              value: 200,
              message: 'Maximum length is 200 characters',
            },
          }}
        />
        <MarkdownField control={control} name="content" />
        <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
          {isSubmitting ? 'Submitting...' : 'Submit'}
        </FormButton>
      </form>
    </div>
  )
}
