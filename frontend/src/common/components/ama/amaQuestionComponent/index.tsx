import { FunctionComponent } from 'react'
import { AmaQuestion } from '../../../../models/amaSessions'
import defaultImgUrl from '/person-icon.png'

import CustomImage from '../../customImage'
import { useToggle } from 'usehooks-ts'
import { ToggleButton } from '../../shared/ToggleButton'
import { AuthorizedView } from '../../auth/AuthorizedView'
import { AmaQuestionReplyForm } from './AmaQuestionReplyForm.tsx'

type Props = {
  page: number
  question: AmaQuestion
  managingUserId: string
  parentQueryKey: unknown[]
}

export const AmaQuestionComponent: FunctionComponent<Props> = ({
  // page,
  question,
  managingUserId,
  parentQueryKey,
}) => {
  const [isReplying, toggleIsReplying] = useToggle()
  return (
    <div className="my-3 flex gap-6 rounded-xl bg-white p-3 shadow-md">
      <div className="flex max-w-min flex-col">
        <CustomImage
          className="picture aspect-square w-16 w-24 min-w-16 rounded-full border-2 object-cover shadow-md md:min-w-24"
          src={`data:image/webp;base64,${question.profilePicture || defaultImgUrl}`}
        />
        <div className="mb-3 max-w-full text-left">
          asked by: <span className="block font-bold">{question.username}</span>
        </div>
      </div>
      <div className="flex flex-1 flex-col whitespace-pre-line">
        <div className="flex-1">
          <div>{question.content}</div>
          <div className="qu ml-8 mt-2 rounded-md border-[1px] border-slate-200 bg-slate-50 px-2 py-1 shadow-md">
            {question.answer}
          </div>
        </div>
        <div className="flex">
          <AuthorizedView requiredUserId={managingUserId}>
            <ToggleButton isToggled={isReplying} toggle={toggleIsReplying}>
              Reply
            </ToggleButton>
          </AuthorizedView>
        </div>
        {isReplying && (
          <AmaQuestionReplyForm
            question={question}
            parentQueryKey={parentQueryKey}
            toggleIsReplying={toggleIsReplying}
          />
        )}
      </div>
    </div>
  )
}
