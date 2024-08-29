import { createFileRoute } from '@tanstack/react-router'
import AuthorForm from '../../../common/components/authors/AuthorForm.tsx'
import { useEffect } from 'react'
import axios from 'axios'

export const Route = createFileRoute('/authors/create/')({
  component: () => {
    useEffect(() => {
      axios.get('/book/me').then((res) => console.log(res.data))
    }, [])

    return <AuthorForm />
  },
})
