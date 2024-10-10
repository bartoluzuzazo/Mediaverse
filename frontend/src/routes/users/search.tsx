import { createFileRoute } from '@tanstack/react-router'
import { UserSearch } from '../../common/components/users/UserSearch'


export const Route = createFileRoute('/users/search')({
	component: ()=><div className="mt-6"><UserSearch/></div>
})