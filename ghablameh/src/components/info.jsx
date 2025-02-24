import React, { useState, useEffect } from 'react';
import styles from '../styles/updateinfo.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { toast } from 'react-toastify';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import axios from 'axios';
import Navbarparent from './navbarparent';
import Avatar from "react-avatar";
import Select from "react-select";
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import '../styles/customNotifications.css';

import {useNavigate } from "react-router-dom";

const Update = () => {

  const [birthdate, setBirthdate] = useState('');
  const [gender, setGender] = useState('');
  const [username, setUsername] = useState('');
  const [firstName, setfirstName] = useState('');
  const [lastName, setlastName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setphoneNumber] = useState('');

  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const [image_base64, setImage_base64] = useState('');
  const [image_url, setImage_url] = useState('');

  const [avatar, setAvatar] = useState('');
  const [formErrors, setFormErrors] = useState([]);
  const [token, setToken] = useState('');
  const [error, setError] = useState('');
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const [isWaitingForm1, setIsWaitingForm1] = useState(false);
  const [isWaitingForm2, setIsWaitingForm2] = useState(false);

  const [passErrors, setPassErrors] = useState([]);
  const navigate = useNavigate();
  const options = [
    { value: 'M', label: 'مرد' },
    { value: 'F', label: 'زن' }
  ];

  // fetch user data
  useEffect(() => {
    const FetchData = async () => {
      try {
        const response = await axios.get('https://ghablameh.fiust.ir/api/v1/clients/me/', {
          headers: {
            'Authorization': 'JWT ' + localStorage.getItem("token")
          }
        });

        if (response.status === 200) {

          const genderOption = options.find(option => option.value == response.data.gender);
          setGender(genderOption || null);

          setBirthdate(response.data.birthdate || '')
          setUsername(response.data.username || '');
          setfirstName(response.data.first_name || '');
          setlastName(response.data.last_name || '');
          setEmail(response.data.email || '');
          setphoneNumber(response.data.phone_number || '');
          setImage_url(response.data.image_url || '')
          console.log(response.data)
        } else {
          console.log('fetching userData failed:', response.status);
        }
      } catch (error) {
        console.error('An error occurred:', error);
      }

    };
    FetchData();
  }, []);

  const toggleCurrentPasswordVisibility = () => {
    setShowCurrentPassword(!showCurrentPassword);
  };

  const toggleNewPasswordVisibility = () => {
    setShowNewPassword(!showNewPassword);
  };

  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const createNotification = (type,error) => {
    return () => {
      switch (type) {
        case 'fail':
          NotificationManager.error(`${error}`, '', 3000);
          break;
        case 'success':
          NotificationManager.success(' رفتن به صفحه اصلی ','اطلاعات با موفقیت ثبت شد ',  3000,() => {navigate("/")});
          break;

        default:
          break;
      }
    };
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();

    const errors = [];

    setFormErrors('');

    let engNumbers = {
      '۰': 0,
      '۱': 1,
      '۲': 2,
      '۳': 3,
      '۴': 4,
      '۵': 5,
      '۶': 6,
      '۷': 7,
      '۸': 8,
      '۹': 9
    };

    if (!birthdate) {
      errors.push('تاریخ تولد را وارد کنید');
      createNotification('fail','تاریخ تولد را وارد کنید')();
    }

    if (!gender) {
      errors.push('جنسیت را انتخاب کنید');
      createNotification('fail','جنسیت را انتخاب کنید')();
    }

    if (!username) {
      errors.push('نام کاربری را وارد کنید');
      createNotification('fail','نام کاربری را وارد کنید')();
    }

    if (newPassword !== confirmPassword) {
      errors.push('رمز عبور جدید و تأیید رمز عبور مطابقت ندارند');
      createNotification('fail','رمز عبور جدید و تأیید آن مطابقت ندارند')();
    }

    if (!/^([a-zA-Z0-9!_.]+)@([a-zA-Z]+)\.([a-zA-Z]{2,})$/.test(email)) {
      errors.push(' ایمیل را به درستی وارد کنید.');
      createNotification('fail','ایمیل را به درستی وارد کنید.')();
    }
    if (phoneNumber.startsWith('98') && phoneNumber.length !== 12) {
      errors.push('شماره را به درستی وارد کنید.');
      createNotification('fail','شماره را به درستی وارد کنید.')();
    }
    if (phoneNumber.startsWith('09') && phoneNumber.length !== 11) {
      errors.push('شماره را به درستی وارد کنید.');
      createNotification('fail','شماره را به درستی وارد کنید.')();
    }

    let phoneNumber_english = phoneNumber.replace(/[۰-۹]/g, function (w) {
      return engNumbers[w]
    });

    // check if admin_phone_number contains only numbers and starts with '989' or '09'
    if (!/^\d+$/.test(phoneNumber_english) || !/^(989|09)/.test(phoneNumber_english)) {
      errors.push('شماره را به درستی وارد کنید.');
      createNotification('fail','شماره را به درستی وارد کنید.')();
    }

    // if (errors.length > 0) {
    //   alert(errors.join('\n'));
    //   setFormErrors(errors);
    //   return;
    // }
    if (errors.length > 0) {
      // toast.error(errors.join('\n'));

      // createNotification('fail',errors.join('\n'))();
      setFormErrors('');
      return;
    }

    const userData = {
      gender: gender.value,
      birthdate: birthdate,
      first_name: firstName,
      last_name: lastName,
      username: username,
      email: email,
      phone_number: phoneNumber_english,
    };

    if (image_base64) {
      userData.image_base64 = image_base64;
    }

    //send form data
    setIsWaitingForm1(true);
    try {
      console.log("userData: ", userData);

      const response = await axios.put('https://ghablameh.fiust.ir/api/v1/clients/me/', userData, {
        headers: { Authorization: "JWT " + localStorage.getItem("token") }
      });

      if (response.status === 200) {
        setIsWaitingForm1(false);
        // alert('اطلاعات با موفقیت ثبت شد ');
        createNotification('success')();
        // window.location.href = '/';
      } else {
        const errorData = await response.json();
        setIsWaitingForm1(false);
        // alert(response.message);
        createNotification('fail',response.message)();
      }
    } catch (error) {
      setIsWaitingForm1(false);
      // alert(error.message);

      if (error.response.data.phone_number) {
        // alert(error.response.data.phone_number);
        createNotification('fail',error.response.data.phone_number)();
      }
      if (error.response.data.first_name) {
        // alert(error.response.data.first_name);
        createNotification('fail',error.response.data.first_name)();
      }
      if (error.response.data.username) {
        // alert(error.response.data.username);
        createNotification('fail',error.response.data.username)();
      }
      if (error.response.data.last_name) {
        // alert(error.response.data.last_name);
        createNotification('fail',error.response.data.last_name)();
      }
      if (error.response.data.email) {
        // alert(error.response.data.email);
        createNotification('fail',error.response.data.email)();
      }

      // console.error(error.response.data)
    }
    setFormErrors('');
    setIsWaitingForm1(false);
  };

  const handlePassSubmit = async (e) => {
    e.preventDefault();

    const errors = [];
    setPassErrors('');

    if (!currentPassword) {
      errors.push('رمز عبور فعلی را وارد کنید');
      createNotification('fail','رمز عبور فعلی را وارد کنید')();
    }

    if (!confirmPassword) {
      errors.push('تأیید رمز عبور جدید را وارد کنید');
      createNotification('fail','تأیید رمز عبور جدید را وارد کنید')();
    }

    if (newPassword !== confirmPassword) {
      errors.push('رمز عبور جدید و تأیید رمز عبور مطابقت ندارند');
      createNotification('fail','رمز عبور جدید و تأیید آن مطابقت ندارند')();
    }
    if (!/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(newPassword)) {
      errors.push(' رمز عبور باید حداقل ۸ کاراکتر و شامل اعداد و حروف باشد. ');
      createNotification('fail',' رمز عبور باید حداقل ۸ کاراکتر و شامل اعداد و حروف باشد. ')();
    }

    if (errors.length > 0) {
      // toast.error(errors.join('\n'));
      setFormErrors('');
      return;
    }

    const passwordData = {
      old_password: currentPassword,
      new_password: newPassword,
    };
    setIsWaitingForm2(true);
    try {
      const token = 'JWT ' + localStorage.getItem("token");

      const response = await axios.post('https://ghablameh.fiust.ir/api/v1/clients/password/', passwordData, {
        headers: {
          'Authorization': token
        }
      });

      if (response.status === 200) {
        setIsWaitingForm2(false);
         // alert('اطلاعات با موفقیت ثبت شد ');
         createNotification('success')();
         // window.location.href = '/';
      } else {
        const errorData = await response.json();
        setIsWaitingForm2(false);
        // alert(' مشکلی پیش امده.لطفا در زمانی دیگر امتحان کنید ')
        createNotification('fail',' مشکلی پیش امده.لطفا در زمانی دیگر امتحان کنید ')();
      }
    } catch (error) {
      // console.error('An error occurred:', error);
      setIsWaitingForm2(false);
      if (error.response.data.old_password) {
        // alert(error.response.data.old_password[0]);
        createNotification('fail',error.response.data.old_password[0])();
      } else if (error.response.data.new_password) {
        // alert(error.response.data.new_password[0]);
        createNotification('fail',error.response.data.new_password[0])();
      } else if (error.response.data.new_password & error.response.data.old_password) {
        // alert(error.response.data.new_password[0]);
        // alert(error.response.data.old_password[0]);
        createNotification('fail',error.response.data.new_password[0])();
        createNotification('fail',error.response.data.old_password[0])();
      }
    }
    setFormErrors('');
    setIsWaitingForm2(false);
  };

  //image
  const handlePhotoChange = (event) => {
    let reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = (e) => {
      let img = e.target.result;
      setImage_base64(img);
    };
  };

  const clearPhoto = () => {
    setImage_base64('');
    setImage_url('');
  }

  const handleChangeGender = (selectedOption) => {
    setGender(selectedOption);
  }
  return (

    <div className={styles.bg}>
      <Navbarparent />

      <NotificationContainer />
      <div className={styles.container}>
        <div className={styles.pattern}></div>
        <div className={styles.card}>
          <h2 className={styles.title}>به‌روزرسانی اطلاعات کاربر</h2>

          {/* info */}
          <form onSubmit={handleFormSubmit} className={styles.form}>
            {formErrors.length > 0 && (
              <div className={styles.errorContainer}>
                <ul className={styles.errorList}>
                  {formErrors.map((error, index) => (
                    <li key={index}>{error}</li>
                  ))}
                </ul>
              </div>
            )}

            {/* image */}
            <div className='flex flex-col items-center justify-center'>
              <div className={styles.formGroup}>
                <div className={styles.avatarimg}>
                  <div className='flex flex-col items-center justify-center'>
                    <label htmlFor="file_input">

                      {image_base64 || image_url ?
                        <img
                          src={!image_base64 && image_url.startsWith('/api/')
                            ? `https://ghablameh.fiust.ir/${image_url}`
                            : image_base64}
                          className={styles.avatar}
                          alt="Profile"
                        />
                        :
                        <Avatar
                          name={firstName}
                          size="130"
                          round={true}
                          maxInitials={1}
                        />
                      }

                    </label>
                    <input id="file_input" type="file" onChange={handlePhotoChange} className={styles.fileinput}></input>
                  </div>
                </div>
              </div>
              {/* <button type='reset' onClick={clearPhoto} className='m-2 text-white text-sm rounded-lg p-2' style={{ backgroundColor: "rgb(38, 87, 124)" }}> حذف عکس </button> */}
            </div>


            <div className={styles.formGroup}>
              <div className='flex flex-row items-center w-full justify-center mt-10'>
                <div className='ml-2 w-1/2'>
                  <label htmlFor="firstName" className={styles.label}>
                    نام
                  </label>
                  <input
                    type="text"
                    id="firstname"
                    value={firstName}
                    onChange={(e) => setfirstName(e.target.value)}
                    className={styles.input}
                    style={{ borderRadius: '10px' }}
                    required
                  />
                </div>
                <div className='w-1/2'>
                  <label htmlFor="lastName" className={styles.label}>
                    نام خانوادگی
                  </label>
                  <input
                    type="text"
                    id="lastname"
                    value={lastName}
                    onChange={(e) => setlastName(e.target.value)}
                    className={styles.input}
                    style={{ borderRadius: '10px' }}
                    required
                  />
                </div>
              </div>
            </div>

            <div className='flex flex-row items-center w-full justify-center mt-6'>
              <div className='ml-2 w-1/2'>
                <div className={styles.formGroup}>
                  <label htmlFor="username" className={styles.label}>
                    نام کاربری
                  </label>
                  <input
                    type="text"
                    id="username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    className={styles.input}
                    style={{ borderRadius: '10px', direction: 'ltr' }}
                    required
                  />
                </div>
              </div>

              <div className='w-1/2'>
                <div className={styles.formGroup}>
                  <label htmlFor="phoneNumber" className={styles.label}>
                    شماره تلفن
                  </label>
                  <input
                    type="text"
                    id="phonenumber"
                    value={phoneNumber}
                    onChange={(e) => setphoneNumber(e.target.value)}
                    className={styles.input}
                    style={{ borderRadius: '10px', direction: 'ltr' }}
                    required
                  />
                </div>
              </div>
            </div>


            <div className='flex flex-row items-center w-full justify-center mt-2'>
              <div className='ml-2 w-1/2'>
                <div className={styles.formGroup}>
                  <label htmlFor="birthdate" className={styles.label}>
                    تاریخ تولد
                  </label>
                  <input
                    type="date"
                    id="birthdate"
                    value={birthdate}
                    onChange={(e) => setBirthdate(e.target.value)}
                    className={styles.input}
                    style={{ borderRadius: '10px' }}
                    required
                  />
                </div>
              </div>
              <div className='w-1/2'>
                <div className={styles.formGroup}>
                  <label htmlFor="gender" className={styles.label}>
                    جنسیت
                  </label>
                  <Select
                    options={options}
                    id="gender"
                    value={gender}
                    onChange={handleChangeGender}
                    styles={{
                      control: (styles) => ({
                        ...styles,
                        padding: '3px',
                        borderColor: '#6c727f',
                        borderRadius: '10px'
                      }),
                    }}
                    theme={(theme) => ({
                      ...theme,
                      colors: {
                        ...theme.colors,
                        text: 'de6016',
                        primary: 'rgb(38, 87, 124)',
                        primary25: 'rgba(38, 87, 124,0.4)',
                      }
                    })}
                    required
                  />
                </div>
              </div>
            </div>


            <div className={styles.formGroup}>
              <label htmlFor="email" className={styles.label}>
                ایمیل
              </label>
              <input
                type="text"
                id="email"
                // dir="ltr"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className={styles.input}
                style={{ borderRadius: '10px', direction: 'ltr' }}
                required
              />
            </div>
            {/* submit button */}
            {!isWaitingForm1 && (
              <button type="submit" className={styles.button}>
                ارسال اطلاعات
              </button>
            )}
            {isWaitingForm1 && (
              <button type="submit" className={styles.button}>
                <div className={`${styles.spinner2}`}></div>
              </button>
            )}
          </form>

          {/* pass */}
          <form onSubmit={handlePassSubmit} className={`${styles.form}`}>

            {/* password title */}
            <div className='m-3'>
              <hr style={{ borderTop: '1px solid rgb(38, 87, 124)', marginBottom: '20px', marginTop: '40px' }} />
              <h2 className={styles.title}>به‌روزرسانی رمز عبور </h2>
            </div>

            {/* password fields */}
            <div className={styles.formGroup}>
              <label htmlFor="currentPassword" className={styles.label}>
                رمز عبور فعلی
              </label>
              <div className={styles.passwordInputContainer}>
                <input
                  type={showCurrentPassword ? 'text' : 'password'}
                  id="currentPassword"
                  value={currentPassword}
                  onChange={(e) => setCurrentPassword(e.target.value)}
                  className={styles.input}
                  style={{ borderRadius: '10px' }}
                  required
                />
                <FontAwesomeIcon
                  icon={showCurrentPassword ? faEye : faEyeSlash}
                  className={styles.passwordIcon}
                  onClick={toggleCurrentPasswordVisibility}
                />
              </div>
            </div>
            <div className={styles.formGroup}>
              <label htmlFor="newPassword" className={styles.label}>
                رمز عبور جدید
              </label>
              <div className={styles.passwordInputContainer}>
                <input
                  type={showNewPassword ? 'text' : 'password'}
                  id="newPassword"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  className={styles.input}
                  style={{ borderRadius: '10px' }}
                  required
                />
                <FontAwesomeIcon
                  icon={showNewPassword ? faEye : faEyeSlash}
                  className={styles.passwordIcon}
                  onClick={toggleNewPasswordVisibility}
                />
              </div>
            </div>
            <div className={styles.formGroup}>
              <label htmlFor="confirmPassword" className={styles.label}>
                تأیید رمز عبور جدید
              </label>
              <div className={styles.passwordInputContainer}>
                <input
                  type={showConfirmPassword ? 'text' : 'password'}
                  id="confirmPassword"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className={styles.input}
                  style={{ borderRadius: '10px' }}
                  required
                />
                <FontAwesomeIcon
                  icon={showConfirmPassword ? faEye : faEyeSlash}
                  className={styles.passwordIcon}
                  onClick={toggleConfirmPasswordVisibility}
                />
              </div>
            </div>
            {/* submit button */}
            {!isWaitingForm2 && (
              <button type="submit" className={styles.button}>
                به روز رسانی رمز عبور
              </button>
            )}
            {isWaitingForm2 && (
              <button type="submit" className={styles.button}>
                <div className={`${styles.spinner2}`}></div>
              </button>
            )}
          </form>
        </div>
        {/* <ToastContainer /> */}
      </div>
    </div>
  );
};

export default Update;