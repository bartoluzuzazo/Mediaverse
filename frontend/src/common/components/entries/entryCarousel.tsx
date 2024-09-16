import { FunctionComponent } from 'react'
import Carousel from 'react-multi-carousel'
import { Entry } from '../../../models/entry/Entry'
import { IoIosArrowBack, IoIosArrowForward } from 'react-icons/io'
import classNames from 'classnames'
import { Link } from '@tanstack/react-router'
import CustomImage from '../customImage'

interface EntryCarouselProps {
  entries: Entry[]
  title: string
}

const responsive = {
  desktop: {
    breakpoint: {
      max: 3000,
      min: 1024,
    },
    items: 6,
    partialVisibilityGutter: 40,
  },
  mobile: {
    breakpoint: {
      max: 464,
      min: 0,
    },
    items: 1,
    partialVisibilityGutter: 30,
  },
  tablet: {
    breakpoint: {
      max: 1024,
      min: 464,
    },
    items: 2,
    partialVisibilityGutter: 30,
  },
}

const EntryCarousel: FunctionComponent<EntryCarouselProps> = ({
  entries,
  title,
}) => {
  if (entries.length <= 0) {
    return <></>
  }

  return (
    <div>
      <p className="select-none text-2xl font-bold">{title}</p>
      <Carousel
        additionalTransfrom={0}
        arrows
        autoPlaySpeed={3000}
        centerMode={false}
        className="select-none"
        containerClass="container"
        customLeftArrow={<CarouselCustomArrow direction="left" />}
        customRightArrow={<CarouselCustomArrow direction="right" />}
        dotListClass=""
        focusOnSelect={false}
        itemClass=""
        keyBoardControl
        minimumTouchDrag={80}
        pauseOnHover
        renderArrowsWhenDisabled={false}
        renderButtonGroupOutside={false}
        renderDotsOutside={false}
        responsive={responsive}
        rewind={false}
        rewindWithAnimation={false}
        rtl={false}
        shouldResetAutoplay
        showDots={false}
        sliderClass=""
        slidesToSlide={1}
        swipeable
      >
        {entries.map(({ id, photo }) => (
          <Link
            key={id}
            className="m-1 my-3 block h-[235px] w-[170px] overflow-hidden transition-shadow"
            to="/entries/books/$id"
            params={{ id }}
          >
            <CustomImage
              className="h-full w-full transition-all hover:scale-[1.1]"
              src={`data:image/webp;base64,${photo}`}
            />
          </Link>
        ))}
      </Carousel>
    </div>
  )
}

interface CarouselCustomArrowProps {
  direction: 'left' | 'right'
  onClick?: () => void
}

const CarouselCustomArrow: FunctionComponent<CarouselCustomArrowProps> = ({
  direction,
  onClick,
}) => {
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

export default EntryCarousel
