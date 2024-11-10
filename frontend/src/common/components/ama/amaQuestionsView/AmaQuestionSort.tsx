import { Dispatch, FunctionComponent, SetStateAction } from 'react'
import {
  AmaQuestionOrder,
  GetAmaQuestionParams,
} from '../../../../models/amaSessions'
import { DropdownButton, DropDownChoice } from '../../shared/DropdownChoice'
import { FaArrowDown, FaArrowUp, FaClock, FaMedal } from 'react-icons/fa'
import { OrderDirection } from '../../../../models/common'

type Props = {
  setQuestionParams: Dispatch<
    SetStateAction<Omit<GetAmaQuestionParams, 'page' | 'status'>>
  >
}

export const AmaQuestionSort: FunctionComponent<Props> = ({
  setQuestionParams,
}) => {
  const onSortDropdownChange = (order: AmaQuestionOrder) => {
    setQuestionParams((params) => {
      return { ...params, order }
    })
  }

  const SortDropdownOptions = [
    {
      element: <DropdownButton icon={<FaMedal />} text="votes" />,
      value: AmaQuestionOrder.TotalVotes,
    },
    {
      element: <DropdownButton icon={<FaClock />} text="time" />,
      value: AmaQuestionOrder.CreatedAt,
    },
  ]

  const onDirectionDropdownChange = (direction: OrderDirection) => {
    setQuestionParams((params) => {
      return { ...params, direction }
    })
  }

  const directionDropdownOptions = [
    {
      element: <DropdownButton icon={<FaArrowDown />} text="descending" />,
      value: OrderDirection.Descending,
    },
    {
      element: <DropdownButton icon={<FaArrowUp />} text="ascending" />,
      value: OrderDirection.Ascending,
    },
  ]
  return (
    <div className="flex flex-wrap items-center gap-x-12 gap-y-3">
      <div className="flex items-center">
        <span className="mr-3 text-lg font-semibold text-slate-700">
          Sort by:
        </span>
        <DropDownChoice
          options={SortDropdownOptions}
          onChange={onSortDropdownChange}
        />
      </div>
      <div className="flex items-center">
        <span className="mr-3 text-lg font-semibold text-slate-700">
          Direction
        </span>
        <DropDownChoice
          options={directionDropdownOptions}
          onChange={onDirectionDropdownChange}
        />
      </div>
    </div>
  )
}
