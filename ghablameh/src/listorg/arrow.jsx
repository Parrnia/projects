import React, { useRef, useEffect, useState } from "react";
import Slider from "react-slick";
import styles from './FlexLayout.module.css';
import axios from "axios";
import './pagination.css'
const AutoPlayMethods = () => {
  const [topOrganizations, setTopOrganizations] = useState([]);
  const sliderRef = useRef(null);

  const fetchTopOrganizations = async () => {
    try {
      const response = await axios.get('https://ghablameh.fiust.ir/api/v1/organizations/');
      const sortedOrgs = response.data.sort((a, b) => b.average_rate - a.average_rate).slice(0, 5);
      setTopOrganizations(sortedOrgs);
    } catch (error) {
      console.error('Error retrieving organizations:', error);
    }
  };

  useEffect(() => {
    fetchTopOrganizations();

    const play = () => {
      sliderRef.current?.slickPlay();
    };

    const pause = () => {
      sliderRef.current?.slickPause();
    };

    play();

    return () => {
      pause();
    };
  }, []);

  const settings = {
    dots: true,
    infinite: true,
    slidesToShow: 3,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 1000,
    prevArrow: (
      <button
        type="button"
        className="slick-prev slick-arrow text-blue-500 hover:text-blue-800 text-4xl px-4 py-2 rounded-full shadow-md"
      >
        ←
      </button>
    ),
    nextArrow: (
      <button
        type="button"
        className="slick-next slick-arrow text-blue-500 hover:text-blue-800 text-4xl px-4 py-2 rounded-full shadow-md"
      >
        →
      </button>
    )
  };

  return (
    <div className={styles.sliderContainer}>
      <link
        rel="stylesheet"
        type="text/css"
        charSet="UTF-8"
        href="https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick.min.css"
      />
      <link
        rel="stylesheet"
        type="text/css"
        href="https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick-theme.min.css"
      />

      <Slider className={styles.slide} ref={sliderRef} {...settings}>
        {topOrganizations.map((org, index) => (
          <div key={index} className={styles.slide}>
            <div className="bg-white bg-opacity-50 shadow-md rounded-lg overflow-hidden custom-width" style={{ border: '1px solid rgb(38, 87, 124)', marginRight: '20px' }}>
              {org.image_url  ? (
                <div className="overflow-hidden h-40" style={{ padding: '10px' }}>
                  <img src={'https://ghablameh.fiust.ir' + org.image_url} className="w-full h-full rounded-lg" style={{ objectFit: 'contain' }} />
                </div>
              ) : (
                <div
                  style={{
                    height: "160px",
                    backgroundImage: "repeating-conic-gradient(#26577c  0% 25%, #ffffff 0% 50%)",
                    backgroundPosition: "0 0, 32px 32px",
                    backgroundSize: "50px 50px",
                    opacity: '0.5',
                  }}
                />
              )}
              <h3>{org.name}</h3>
              {/* <p>امتیاز: {org.average_rate}</p> */}
              <p>امتیاز: {org.average_rate !== null ? org.average_rate : 'بدون نظر '}</p>
            </div>
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default AutoPlayMethods;