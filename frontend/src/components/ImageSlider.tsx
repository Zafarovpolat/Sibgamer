import { Swiper, SwiperSlide } from 'swiper/react';
import { Autoplay, Pagination, Navigation, EffectFade } from 'swiper/modules';
import { resolveMediaUrl } from '../lib/media';
import 'swiper/swiper-bundle.css';

interface SliderProps {
  images: Array<{
    id: number;
    imageUrl: string;
    title: string;
    description: string;
    buttons?: Array<{
      Text: string;
      Url: string;
    }>;
  }>;
}

const ImageSlider = ({ images }: SliderProps) => {
  if (!images || images.length === 0) {
    return (
      <div className="h-96 glass-card flex items-center justify-center animate-pulse">
        <p className="text-gray-400">Загрузка слайдера...</p>
      </div>
    );
  }

  return (
    <div className="w-full h-96 rounded-2xl overflow-hidden shadow-2xl">
      <Swiper
        modules={[Autoplay, Pagination, Navigation, EffectFade]}
        spaceBetween={0}
        centeredSlides={true}
        loop={true}
        autoplay={{
          delay: 5000,
          disableOnInteraction: false,
        }}
        effect="fade"
        fadeEffect={{
          crossFade: true,
        }}
        pagination={{
          clickable: true,
          bulletClass: 'swiper-pagination-bullet !bg-white/50',
          bulletActiveClass: 'swiper-pagination-bullet-active !bg-highlight',
        }}
        navigation={true}
        className="h-full"
      >
        {images.map((image) => (
          <SwiperSlide key={image.id}>
            <div className="relative h-full">
              <img
                src={resolveMediaUrl(image.imageUrl)}
                alt={image.title}
                className="w-full h-full object-cover"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-black via-black/50 to-transparent" />
              <div className="absolute bottom-0 left-0 right-0 p-8 md:p-12 animate-slide-in">
                <div className="flex justify-between items-end h-full">
                  <div className="flex-1">
                    <h3 className="text-3xl md:text-5xl font-bold mb-3 bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
                      {image.title}
                    </h3>
                    <p className="text-lg md:text-xl text-gray-300 max-w-3xl mb-6">
                      {image.description}
                    </p>
                  </div>
                  {image.buttons && image.buttons.length > 0 && (
                    <div className="flex flex-col gap-2 ml-4">
                      {image.buttons.map((button, index) => (
                        <a
                          key={index}
                          href={button.Url}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="btn-primary inline-flex items-center gap-2 whitespace-nowrap"
                        >
                          {button.Text}
                        </a>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            </div>
          </SwiperSlide>
        ))}
      </Swiper>
    </div>
  );
};

export default ImageSlider;
