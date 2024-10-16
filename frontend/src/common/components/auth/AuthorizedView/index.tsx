import { FunctionComponent, ReactNode } from 'react'
import { Role } from '../../../../models/user'
import { useAuthContext } from '../../../../context/auth/useAuthContext.ts'

type Props = {
  children: ReactNode
  notAuthView?: ReactNode
  allowedRoles?: Role | Role[]
  requiredUserId?: string

}

export const AuthorizedView: FunctionComponent<Props> = ({
                                                           children,
                                                           notAuthView = null,
                                                           requiredUserId,
                                                           allowedRoles,
                                                         }) => {
  const roles = allowedRoles && (Array.isArray(allowedRoles) ? allowedRoles : [allowedRoles])
  const { authUserData } = useAuthContext()!
  if (!authUserData) {
    return notAuthView
  }
  const hasAllowedRole = !roles || roles.some((role) => authUserData?.roles.includes(role))
  if (!hasAllowedRole) {
    return notAuthView
  }
  if (requiredUserId && authUserData.id !== requiredUserId) {
    return notAuthView
  }
  return children
}