import React  ,{ useState } from 'react';
import './profile.css';
import logo from '../main/assest/Logo_Menue.png';
import arrow from './arrow.png'
import profile from './profile.png'
import img from './wallet.png'
import check from './loading.png'
import check1 from './checkmark.png'
import check2 from './checkmark1.png'
import { CircularProgressbar } from 'react-circular-progressbar';
import 'react-circular-progressbar/dist/styles.css';
const ProfilePage = () => {
  const [horizontalValue, setHorizontalValue] = useState(50); // Default horizontal slider value
  const [verticalValue, setVerticalValue] = useState(75);
  const completionPercentage = 75; 
  return (
    <div className="background">
        <div className="d-flex justify-content-end align-items-center">
        <img src={arrow} alt="Logo" className="arrow" />
                <span className="navbartext">دریک پی</span>
                <img src={logo} alt="Logo" className="navbar-brand me-2" />
             
            </div>
            <div className="container">
      <div className="vertical-box1 left">
        <div className="flex-itembox">
          <label className="labelbox">اقساط پیش رو</label>
          <p className="textbox">تنها با چند کلیک اقساط خود را پرداخت کنید
          </p>
  
        </div>
        
        <div className="flex-itembox" id="flex2">
          <label className="labelbox" id="labelbox2"> بیمه</label>
          <p className="textbox">تنها با چند کلیک استعلام وضعیت بیمه های خود را مشاهده کنید
          </p>
          <div className="button-container">
          <button className="buttonbox">بیمه بدنه</button>
            <button className="buttonbox">بیمه شخص ثالث</button>
            <button className="buttonbox">مانده بدهکاری</button>
          </div>
       
        </div>
      </div>
      <div className="horizontal-box">
        <div className="horizontal-subbox">
        <div className="horizontal-sub">
        <div className="flex-item">
        <span className="title5">وضعیت تکمیل پروفایل</span>
        <div className="flex-item" style={{ display: 'flex', alignItems: 'center' }}>
      <div style={{ width: '120px', height: '120px',  

        marginLeft: '25px' ,     marginTop: '30px'
      }}>
        <CircularProgressbar
          value={completionPercentage}
          text={`${completionPercentage}%`} // Display the percentage inside the circle
          styles={{
            path: {
              stroke: `#73FE7C`, // Customize the color of the progress
            },
            text: {
              fill: '#ffffff', // Text color inside the circle
              fontSize: '16px', // Font size of the text
            },
            trail: {
              stroke: '#d6d6d6', // Background color of the progress circle
            },
          }}
        />
      </div>
     
    </div>
        </div>
        <div className="horizontalSub">
        <span className="title4"> در صورت عدم تایید میتوانید با پشتیبانی ما تماس بگیرید
          
          </span>
        <span className="title1">وضعیت احراز هویت</span>
      <div className="flex-item1">
      <span className="title1">در انتظار تایید</span>
      <img src={check} alt="Description" className="image2" />
      </div>
      <div className="flex-item1">
      <span className="title2">تایید شده</span>
      <img src={check1} alt="Description" className="image2" />
          </div>
          <div className="flex-item1">
      <span className="title3">عدم تایید</span>
      <img src={check2} alt="Description" className="image3" />
          </div>
      
      </div>
      
      <div className="horizontalSub">
      <div className="flex-item">
      <span className="title">موجودی کیف پول</span>
      <img src={img} alt="Description" className="image" />
          
          </div>
          <div className="quantity">
          <span className="currency">ریال</span>
          <span className="amount">65 000 000</span>
           
          </div>
          <div className="increaseContainer">
          <span className="increaseText">افزایش موجودی</span>
            <button className="increaseButton">
              +
            </button>
        
          </div>
      </div>
      </div>
        </div>
        <div className="horizontal-subbox1">
          <p className='subtxt'>
            ثبت نام خودرو
          </p>
        </div>
      </div>
      <div className="vertical-box right">
      <img src={profile} alt="plogo" className="plogo" />
      <label className="menulabel">کاربر شماره ۱</label>
      <ul className="vertical-links">
        <li>
        <a href="#" className='linktext'>کیف پول</a>
          <span className="circle"></span>
          
        </li>
        <li>
        <a href="#"  className='linktext'>اعتبار دهی </a>
          <span className="circle"></span>
         
        </li>
        <li>
           <a href="#"  className='linktext'>ثبت خودرو</a>
          <span className="circle"></span>
     
        </li>
        <li>
           <a href="#"  className='linktext'>ثبت  بیمه</a>
          <span className="circle"></span>
     
        </li>
      </ul>
      </div>
    </div>
    </div>
  );
};

export default ProfilePage;