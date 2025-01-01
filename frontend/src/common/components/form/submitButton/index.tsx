import { FunctionComponent } from 'react'
import FormButton from '../button'

interface SubmitButtonProps {
  isSubmitting?: boolean
}

export const SubmitButton: FunctionComponent<SubmitButtonProps> = ({
  isSubmitting,
}) => {
  return (
    <FormButton buttonProps={{ type: 'submit' }} buttonType="purple">
      {isSubmitting ? 'Submitting...' : 'Submit'}
    </FormButton>
  )
}
