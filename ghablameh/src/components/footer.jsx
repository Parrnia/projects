// import '../App.css'
// import styles from '../styles/footer.module.css';
// import React from 'react';
// import { faPhone } from '@fortawesome/free-solid-svg-icons';
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
// import { faEnvelopeOpen } from '@fortawesome/free-solid-svg-icons';
// import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
// import { faTelegramPlane } from '@fortawesome/free-brands-svg-icons';
// import { faFacebookF } from '@fortawesome/free-brands-svg-icons';
// import { faTwitter } from '@fortawesome/free-brands-svg-icons';
// import { faInstagram } from '@fortawesome/free-brands-svg-icons';
// import { faTelegram } from '@fortawesome/free-brands-svg-icons';
// const Footer = () => {
//   return (

//     <div className={styles.app}>
//       <footer className={`${styles['footersection']} flex flex-col justify-center`}>
//         <div className='w-full flex justify-center items-center'>
//           <div className={`${styles.container} flex justify-center items-center`}>
//             <div className={`${styles['footer-content']} pt-5 pb-5 flex items-center justify-center`}>
//               <div className={`${styles['iner-footer']} flex  justify-between`}>


//                 {/* <div className="felx justify-start mb-10 m-5">
//                   <div className={styles['footer-widget']}>
//                     <div className='flex flex-col justify-end items-center'> */}
//                 {/* <div className={styles['footer-logo']}>
//                         <a href="index.html"> */}
//                 {/* https://i.ibb.co/QDy827D/ak-logo.png */}
//                 {/* <img src={'../src/images/logo2.jpg'} className="img-fluid" alt="logo" style={{ height: '5rem' }} /> */}
//                 {/* </a>
//                       </div> */}
//                 {/* <div className={`${styles['footer-social-icon']} flex flex-col items-center  text-white`}>
                        
                        
//                       </div>
//                     </div>
//                   </div>
//                 </div> */}




//                 <div className="mb-10 m-5">
//                   <div className={styles['footer-widget']}>
//                     <div className={styles['footer-widget-heading']}>
//                       <h3 className='font-bold text-white'> شبکه های اجتماعی</h3>
//                       <hr className='mb-5' style={{ borderTop: '2px solid rgb(249 115 22)' }}></hr>
//                     </div>
//                     <div className='flex flex-row items-center'>
//                       <a href="#"><FontAwesomeIcon icon={faFacebookF} className={`${styles['facebook-bg']} text-white`} /></a>
//                       <a href="#"><FontAwesomeIcon icon={faTwitter} className={`${styles['twitter-bg']} text-white`} /></a>
//                       <a href="#"><FontAwesomeIcon icon={faInstagram} size='xs' className={`${styles['insta-bg']} text-white`} style={{width:''}} /></a>
//                       <a href="#"><FontAwesomeIcon icon={faTelegram} className={`${styles['tel-bg']} text-white`} /></a>
//                     </div>
//                   </div>
//                 </div>


//                 <div className="mb-10 m-5">
//                   <div className={styles['footer-widget']}>
//                     <div className={styles['footer-widget-heading']}>
//                       <h3>لینک های مفید</h3>
//                       <hr className='mb-5' style={{ borderTop: '2px solid rgb(249 115 22)' }}></hr>
//                     </div>
//                     <ul>
//                       <li><a href="#">خانه</a></li>
//                       <li><a href="#">درباره</a></li>
//                       <li><a href="#">سرویس ها</a></li>
//                       {/* <li><a href="#">ارتباط</a></li> */}
//                       {/* <li><a href="#">سرویس های ما</a></li> */}
//                       <li><a href="#">تیم متخصص</a></li>
//                       <li><a href="#">ارتباط با ما</a></li>
//                       <li><a href="#">اخرین خبرها</a></li>
//                     </ul>
//                   </div>
//                 </div>

//                 <div className="mb-10 m-5">
//                   <div className={styles['footer-widget']}>
//                     <div className={styles['footer-widget-heading']}>
//                       <h3>ارتباط با ما</h3>
//                       <hr className='mb-5' style={{ borderTop: '2px solid rgb(249 115 22)' }}></hr>
//                     </div>
//                     <div className={styles.footer}>
//                       <div className="flex items-center">
//                         <FontAwesomeIcon
//                           icon={faMapMarkerAlt}
//                           className={styles['custom-icon']}
//                           style={{ color: 'rgb(249 115 22)', fontSize: '24px' }}
//                         />
//                         <div className={`${styles['cta-text']} ml-2`}>
//                           <h4>آدرس ما</h4>
//                           <span>تهران ، خیابان فرجام ، دانشگاه علم و صنعت ایران</span>
//                         </div>
//                       </div>
//                       <div className="flex items-center">
//                         <FontAwesomeIcon
//                           icon={faPhone}
//                           className={styles['custom-icon']}
//                           style={{ color: 'rgb(249 115 22)', fontSize: '24px' }}
//                         />
//                         <div className={`${styles['cta-text']} ml-2`}>
//                           <h4>تماس با ما</h4>
//                           <span>9876543210+ </span>
//                         </div>
//                       </div>
//                       <div className="flex items-center">
//                         <FontAwesomeIcon
//                           icon={faEnvelopeOpen}
//                           className={styles['custom-icon']}
//                           style={{ color: 'rgb(249 115 22)', fontSize: '24px' }}
//                         />
//                         <div className={`${styles['cta-text']} ml-2`}>
//                           <h4>ایمیل ما</h4>
//                           <span>ghableme@example.com</span>
//                         </div>
//                       </div>
//                     </div>

//                   </div>
//                 </div>

//               </div>
//             </div>
//           </div></div>

//         <div className={`${styles['copyright-area']} flex justify-center items-center`}>
//           <div className={styles['container']}>
//             <div className="row">
//               <div className=" lg:text-left ml-9 mb-2">
//               </div>


//               <div className="flex justify-center items-center">
//                 <div className="lg:block text-center mb-4">
//                   <div className={`${styles['footer-menu']}`}>
//                     <ul>
//                       <li>
//                         <a href="#">Home</a>
//                       </li>
//                       <li>
//                         <a href="#">Terms</a>
//                       </li>
//                       <li>
//                         <a href="#">Privacy</a>
//                       </li>
//                       <li>
//                         <a href="#">Policy</a>
//                       </li>
//                       <li>
//                         <a href="#">Contact</a>
//                       </li>
//                     </ul>
//                   </div>
//                 </div>
//               </div>


//             </div>
//           </div>
//         </div>
//       </footer>
//     </div>
//   );
// };

// export default Footer;



import '../App.css'
import styles from  '../styles/footer.module.css';
import React from 'react';
import { faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelopeOpen } from '@fortawesome/free-solid-svg-icons';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { faTelegramPlane } from '@fortawesome/free-brands-svg-icons';
import { faFacebookF } from '@fortawesome/free-brands-svg-icons';
import { faTwitter } from '@fortawesome/free-brands-svg-icons';
import { faInstagram } from '@fortawesome/free-brands-svg-icons';
import { faTelegram} from '@fortawesome/free-brands-svg-icons';
import {useState} from 'react';
import useMediaQuery from '@mui/material/useMediaQuery';

const Footer = () => {
  const isBigScreen = useMediaQuery('(min-width: 1000px)')
  const isMediumScreen = useMediaQuery('(min-width: 900px)')
  const isSmallScreen = useMediaQuery('(min-width: 300px)')

  let widthhh;
  // if (isBigScreen) {
  //   widthhh = '3vw';
  // }
   if (isMediumScreen) {
    widthhh = '25vw';
  }else if(isSmallScreen){
    widthhh = '55vw';
  }


  return (
   
    <div className={styles.app}>
    <footer className={styles.footersection}>
      <div className={`${styles.container} mx-auto`}>
        <div className={`${styles['footer-content']} pt-5 pb-5`}>
          <div className={styles['iner-footer']}>

            <div className="my-2 mr-1 ml-10">
              <div className={styles['footer-widget']}>
                {/* <div className={styles['footer-logo']}>
                  <a href="index.html">
                    <img src="https://i.ibb.co/QDy827D/ak-logo.png" className="img-fluid" alt="logo" />
                  </a>
                </div> */}
                <div className={styles['footer-widget-heading']}>
                  <h3> شبکه های اجتماعی</h3>
                  <hr className='border-orange-500 my-4 w-full' style={{borderTop:'3px solid rgb(255 90 31)'}}></hr>
                 {/* <a href="#"><FontAwesomeIcon icon={faFacebookF} className={`${styles['facebook-bg']} text-white`} /></a> */}
                  <a href="#"><FontAwesomeIcon icon={faTwitter} className={`${styles['twitter-bg']} text-white`} /></a>
                  <a href="#"><FontAwesomeIcon icon={faInstagram} className={`${styles['insta-bg']} text-white`} /></a>
                  <a href="#"><FontAwesomeIcon icon={faTelegram} className={`${styles['tel-bg']} text-white`} /></a>
                </div>
              </div>
            </div>

            <div className="my-2 mr-1 ml-10">
              <div className={styles['footer-widget']}>
                <div className={styles['footer-widget-heading']}>
                  <h3>لینک های مفید</h3>
                  <hr className='border-orange-500 my-4 w-full' style={{borderTop:'3px solid rgb(255 90 31)'}}></hr>
                 
                </div>
                 
                <ul>
                  <li><a href="#">خانه</a></li>
                  <li><a href="#">درباره</a></li>
                  <li><a href="#">سرویس ها</a></li>
                  {/* <li><a href="#">ارتباط</a></li> */}
                  {/* <li><a href="#">سرویس های ما</a></li> */}
                  <li><a href="#">تیم متخصص</a></li>
                  <li><a href="#">ارتباط با ما</a></li>
                  <li><a href="#">اخرین خبرها</a></li>
                </ul>
              </div>
            </div>

            <div className="my-2 mr-1 ml-10">
              <div className={styles['footer-widget']}>
                <div className={styles['footer-widget-heading']}>
                  <h3>ارتباط با ما</h3>
                  <hr className='border-orange-500 my-4 w-full' style={{borderTop:'3px solid rgb(255 90 31)'}}></hr>
                 
                </div>
                <div className=''>
                {/* <hr className='border-orange-500 my-4' style={{borderTop:'3px solid rgb(255 90 31)',  width: widthhh}}></hr> */}
                {/* <hr style={{ width: "50%" }} /> */}
                  </div>
                <div className={styles.footer}>
            <div className="flex items-center">
              <FontAwesomeIcon
                icon={faMapMarkerAlt}
                className={styles['custom-icon']}
                style={{ color: '#ff5e14', fontSize: '24px' }}
              />
              <div className={`${styles['cta-text']} ml-2`}>
                <h4>آدرس </h4>
                <span>تهران، خیابان فرجام، دانشگاه علم و صنعت</span>
              </div>
            </div>
            <div className="flex items-center">
              <FontAwesomeIcon
                icon={faPhone}
                className={styles['custom-icon']}
                style={{ color: '#ff5e14', fontSize: '24px' }}
              />
              <div className={`${styles['cta-text']} ml-2`}>
                <h4>تماس با ما</h4>
                <span>9876543210+ </span>
              </div>
            </div>
            <div className="flex items-center">
              <FontAwesomeIcon
                icon={faEnvelopeOpen}
                className={styles['custom-icon']}
                style={{ color: '#ff5e14', fontSize: '24px' }}
              />
              <div className={`${styles['cta-text']} ml-2`}>
                <h4>ایمیل </h4>
                <span>mail@info.com</span>
              </div>
            </div>
          </div>
                
              </div>
            </div>

          </div>
        </div>
      </div>
     
      {/* <div className={styles['copyright-area']}>
      <div className={styles['container']}>
        <div className="row">
        <div className=" lg:text-left ml-9 mb-2">
          </div>
          <div className="text-right">
          <div className="lg:block col-span-6 lg:col-span-6 text-right mb-4">
            <div className={styles['footer-menu']}>
              <ul>
                <li>
                  <a href="#">Home</a>
                </li>
                <li>
                  <a href="#">Terms</a>
                </li>
                <li>
                  <a href="#">Privacy</a>
                </li>
                <li>
                  <a href="#">Policy</a>
                </li>
                <li>
                  <a href="#">Contact</a>
                </li>
              </ul>
            </div>
            </div>
          </div>
        </div>
      </div>
    </div> */}

    </footer>
    </div>
  );
};

export default Footer;