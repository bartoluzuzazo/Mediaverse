import { createFileRoute } from '@tanstack/react-router'
import { UserSearch } from '../../common/components/users/UserSearch'
import { userService } from '../../services/userService.ts'


export const Route = createFileRoute('/users/search')({
	component: ()=><div className="mt-6"><UserSearch queryKey={"SEARCH_USERS"} searchFunction={userService.search}/></div>
})