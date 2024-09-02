import { FunctionComponent } from 'react'
import Carousel from 'react-multi-carousel'
import { Entry } from '../../../models/entry/Entry'

interface EntryCarouselProps {
  books: Entry[]
}

const responsive = {
  desktop: {
    breakpoint: {
      max: 3000,
      min: 1024,
    },
    items: 3,
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

const EntryCarousel: FunctionComponent<EntryCarouselProps> = ({ books }) => {
  console.log(books)
  return (
    <Carousel
      additionalTransfrom={0}
      arrows
      autoPlaySpeed={3000}
      centerMode={false}
      className=""
      containerClass="container"
      //   customLeftArrow={<CustomLeftArrow />}
      //   customRightArrow={<CustomRightArrow />}
      dotListClass=""
      draggable
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
      {books.map((b) => (
        <img
          className="h-[235px] w-[170px]"
          src={`data:image/png;base64, ${b.photo}`}
        />
      ))}
    </Carousel>
  )
}

export default EntryCarousel
