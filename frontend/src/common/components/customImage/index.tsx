import { FunctionComponent } from 'react'
import { Img, ImgProps } from 'react-image'
import Skeleton from 'react-loading-skeleton'
import { MdNoPhotography } from 'react-icons/md'

interface CustomImageProps extends ImgProps {
}

const CustomImage: FunctionComponent<CustomImageProps> = ({ src, ...rest }) => {
  return (
      <Img
        src={src}
        loader={<Skeleton className="h-full w-full" />}
        unloader={
          <div
            className="flex h-full w-full flex-col items-center justify-center border border-solid border-black text-black">
            <MdNoPhotography className="h-10 w-10" />
            Image Not Found
          </div>
        }
        {...rest}
      />
  )
}

export default CustomImage
