import { FunctionComponent } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { userService } from '../../../../services/userService.ts'
import { RoleStatus } from '../../../../models/user/role'
import { ToggledView } from '../../shared/ToggledView'

type Props = {
  userId: string
}

export const RoleStatusList: FunctionComponent<Props> = ({ userId }) => {
  const { data: roleStatuses } = useQuery({
    queryKey: ['GET_USERS_ROLES', userId],
    queryFn: async () => (await userService.getRoleStatuses(userId)).data,
  })
  if (roleStatuses) {
    return (
      <ToggledView title="Manage roles" containerClass="max-w-[200px]">
        <ul>
          {roleStatuses.map((rs) => (
            <RoleStatusCheckbox userId={userId} roleStatus={rs} key={rs.id} />
          ))}
        </ul>
      </ToggledView>
    )
  }
  return <></>
}

type RSProps = {
  roleStatus: RoleStatus
  userId: string
}

export const RoleStatusCheckbox: FunctionComponent<RSProps> = ({
  roleStatus,
  userId,
}) => {
  const queryClient = useQueryClient()
  const { mutateAsync: postRoleMutate } = useMutation({
    mutationFn: () => userService.postUsersRole(userId, roleStatus.id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['GET_USERS_ROLES', userId],
      })
    },
  })

  const { mutateAsync: deleteRoleMutate } = useMutation({
    mutationFn: () => userService.deleteUsersRole(userId, roleStatus.id),
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['GET_USERS_ROLES', userId],
      })
    },
  })

  const onClick = async () => {
    if (roleStatus.isUsers) {
      await deleteRoleMutate()
    } else {
      await postRoleMutate()
    }
  }
  return (
    <li className="flex items-center gap-2 text-lg font-semibold">
      <input
        type="checkbox"
        checked={roleStatus.isUsers}
        className="aspect-square w-4 rounded-md accent-purple-700 checked:text-purple-700"
        onChange={onClick}
      />
      <p>{roleStatus.name}</p>
    </li>
  )
}
