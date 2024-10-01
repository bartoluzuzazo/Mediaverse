import { FunctionComponent } from 'react'
import classNames from 'classnames'
import { IoIosArrowBack, IoIosArrowForward } from 'react-icons/io'

interface CarouselCustomArrowProps {
  direction: 'left' | 'right'
  onClick?: () => void
}

export const CarouselCustomArrow: FunctionComponent<
  CarouselCustomArrowProps
> = ({ direction, onClick }) => {
  return (
    <div
      onClick={onClick}
      className={classNames(
        'absolute z-10 flex h-full w-[70px] items-center justify-center border-0 outline-none transition-colors hover:cursor-pointer',
        direction === 'left' ? 'left-0' : 'right-0'
      )}
      style={{
        background:
          direction === 'left'
            ? 'linear-gradient(90deg, rgba(255, 255, 255, 100) 0%, rgba(255, 255, 255, 20) 65%, rgba(0, 0, 0, 0) 100%)'
            : 'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, rgba(255, 255, 255, 20) 35%, rgba(255, 255, 255, 100) 100%)',
      }}
    >
      {direction === 'left' ? (
        <IoIosArrowBack className="h-[30px] w-[30px]" />
      ) : (
        <IoIosArrowForward className="h-[30px] w-[30px]" />
      )}
    </div>
  )
}
