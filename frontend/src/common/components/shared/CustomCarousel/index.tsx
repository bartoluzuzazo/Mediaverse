import { CarouselCustomArrow } from './CarouselCustomArrow.tsx'
import Carousel, { ResponsiveType } from 'react-multi-carousel'
import { FunctionComponent } from 'react'

type Props = {
  children: unknown
  customResponsive?: ResponsiveType
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

export const CustomCarousel: FunctionComponent<Props> = ({
  customResponsive,
  children,
}) => {
  return (
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
      responsive={customResponsive || responsive}
      rewind={false}
      rewindWithAnimation={false}
      rtl={false}
      shouldResetAutoplay
      showDots={false}
      sliderClass=""
      slidesToSlide={1}
      swipeable
    >
      {children}
    </Carousel>
  )
}
