import Navbarparent from '../components/navbarparent'
import Navbar from '../components/Navbar';
import styles from './Homepage.module.css';
import { Link, redirect } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
function HomeOrgPage() {
    const [activeSlide, setActiveSlide] = useState(0);
    const [slides, setSlides] = useState([]);

    const API_ENDPOINT = 'https://ghablameh.fiust.ir/api/v1/buffets/';
    const TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzE3MTYxODEzLCJpYXQiOjE3MTQ1Njk4MTMsImp0aSI6ImQxZTM5Y2I4Yzk3ODQxMGFiYjA0NTVkN2U2M2QwMWUwIiwidXNlcl9pZCI6Mn0.CUEgrbFFFKgk5sy7VfqlKaWVqqg5Gv6hSbDZdrIbetA';

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(API_ENDPOINT, {
                    headers: {
                        Authorization: `JWT ${TOKEN}`,
                    },
                });

                const slideData = response.data.slice(0, 5).map((item) => ({
                    text: item.name,
                    color: 'gray-400',
                }));
                setSlides(slideData);
            } catch (error) {
                console.error('Error fetching slide data:', error);
                setSlides([]); // Set an empty array if an error occurs
            }
        };

        fetchData();
    }, []);


    const handlePrevSlide = () => {
        setActiveSlide((prevSlide) => (prevSlide - 1 + slides.length) % slides.length);
    };

    const handleNextSlide = () => {
        setActiveSlide((prevSlide) => (prevSlide + 1) % slides.length);
    };

    useEffect(() => {
        const interval = setInterval(() => {
            setActiveSlide((prevSlide) => (prevSlide + 1) % slides.length);
        }, 5000);

        return () => clearInterval(interval);
    }, [slides.length]
    );

    /*         const slides = [
                { text: 'First slide', color: 'gray-400' },
                { text: 'Second slide', color: 'gray-400' },
                { text: 'Third slide', color: 'gray-400' },
                { text: '4 slide', color: 'gray-400' },
                { text: '5 slide', color: 'gray-400' },
                { text: '6 slide', color: 'gray-400' },
                { text: '7 slide', color: 'gray-400' },
                { text: '8 slide', color: 'gray-400' },
              ]; */
    return (
        <div className={styles.containment_OrgHome}>
            <div className={styles.itemscenter}>
                <div className={styles.app}>
                    <Navbarparent />
                    <div className='grid grid-rows-12 grid-cols-5'>
                        <div className='row-start-1 col-start-1 col-span-6'>
                        </div>
                        <div className='lg:scale-75 md:scale-75 scale-75 lg:col-start-1 lg:col-span-6 lg:row-start-1 lg:row-span-3 md:col-start-1 md:col-span-7 md:row-start-1 md:row-span-3  col-start-1 col-span-5 row-start-1 row-span-3'>
                            <div className='bg-white bg-opacity-60 h-full rounded-lg m-5 grid grid-rows-5 grid-cols-5'>
                                <div className='col-start-2 col-span-3 row-start-1'>
                                    <p className='max font-semibold text-template-custom-blue text-4xl dark:text-template-custom-blue text-center mt-5'>بوفه ها</p>
                                </div>
                                <div className='col-start-2 col-span-3 row-start-2 row-span-3'>
                                    <div data-hs-carousel='{"loadingClasses": "opacity-0", "isAutoPlay": true}' className="relative">
                                        <div className="hs-carousel relative overflow-hidden w-full min-h-96 bg-white rounded-lg">
                                            <div className="hs-carousel-slide flex flex-col">
                                                {slides.map((slide, index) => (
                                                    <div
                                                        key={index}
                                                        className={`flex flex-1 justify-center items-center bg-${slide.color} p-6 ${index === activeSlide ? 'opacity-100' : 'opacity-0'
                                                            }`}
                                                    >
                                                        <span className="text-4xl text-white transition duration-700">
                                                            {slide.text}
                                                        </span>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                        <div className={styles.button_pre_nex}>
                                            <button
                                                type="button"
                                                className="hs-carousel-prev hs-carousel:disabled:opacity-50 disabled:pointer-events-none absolute inset-y-0 start-0 inline-flex justify-center items-center w-[46px] h-full text-gray-800 hover:bg-gray-800/10 rounded-s-lg"
                                                onClick={handlePrevSlide}
                                            >
                                                <span className="text-2xl" aria-hidden="true">
                                                    <svg className="flex-shrink-0 size-5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                                        <path d="m15 18-6-6 6-6"></path>
                                                    </svg>
                                                </span>
                                                <span className="sr-only">Previous</span>
                                            </button>
                                            <button
                                                type="button"
                                                className="hs-carousel-next right-0 left-auto hs-carousel:disabled:opacity-50 disabled:pointer-events-none absolute inset-y-0 end-0 inline-flex justify-center items-center w-[46px] h-full text-gray-800 hover:bg-gray-800/10 rounded-e-lg"
                                                onClick={handleNextSlide}
                                            >
                                                <span className="sr-only">Next</span>
                                                <span className="text-2xl" aria-hidden="true">
                                                    <svg className="flex-shrink-0 size-5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                                        <path d="m9 18 6-6-6-6"></path>
                                                    </svg>
                                                </span>
                                            </button>
                                        </div>
                                        <div className="hs-carousel-pagination flex justify-center absolute bottom-3 right-0 left-0">
                                            {slides.slice().reverse().map((slide, index) => (
                                                <span
                                                    key={index}
                                                    className={`hs-carousel-active:bg-blue-700 hs-carousel-active:border-blue-700 w-3 h-3 rounded-full cursor-pointer ${slides.length - 1 - index === activeSlide ? 'bg-blue-700 border-blue-700' : 'bg-orange-400'
                                                        }`}
                                                    onClick={() => setActiveSlide(slides.length - 1 - index)}
                                                    style={{ marginRight: '10px' }}
                                                ></span>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                                <div className='col-start-2 col-span-3 row-start-5'>
                                    <div className='font-normal text-template-custom-blue textxl'>
                                        <Link to="/OrgPage" className={styles.link_to_Panelorg}>
                                            <p className='text-lg'>مدیریت بوفه ها</p>
                                        </Link>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
export default HomeOrgPage;