import { useState, useEffect } from 'react';
import styles from '../styles/updateorg.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import 'react-toastify/dist/ReactToastify.css';
import axios from 'axios';
import Navbarparent from './navbarparent';
import Avatar from "react-avatar";
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import '../styles/customNotifications.css';
import { useNavigate } from "react-router-dom";

const Update = () => {
  const [name, setName] = useState('');
  const [admin_first_name, setAdmin_first_name] = useState('');
  const [admin_last_name, setAdmin_last_name] = useState('');
  const [admin_username, SetAdmin_username] = useState('');
  const [admin_email, setAdmin_email] = useState('');
  const [admin_phone_number, setAdmin_phone_number] = useState('');
  const [image_base64, setImage_base64] = useState('');
  const [image_url, setImage_url] = useState('');

  const [old_password, setOld_password] = useState('');
  const [new_password, setNew_password] = useState('');
  const [confirm_new_password, setConfirm_new_password] = useState('');

  const [formErrors, setFormErrors] = useState([]);
  const [passErrors, setPassErrors] = useState([]);
  const [isWaitingForm1, setIsWaitingForm1] = useState(false);
  const [isWaitingForm2, setIsWaitingForm2] = useState(false);
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const navigate = useNavigate();

  const toggleCurrentPasswordVisibility = () => {
    setShowCurrentPassword(!showCurrentPassword);
  };

  const toggleNewPasswordVisibility = () => {
    setShowNewPassword(!showNewPassword);
  };

  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };


  const createNotification = (type, error) => {
    return () => {
      switch (type) {
        case 'fail':
          NotificationManager.error(`${error}`, '', 3000);
          break;
        case 'success':
          NotificationManager.success(' رفتن به صفحه اصلی ', 'اطلاعات با موفقیت ثبت شد ', 3000, () => { navigate("/") });
          break;

        default:
          break;
      }
    };
  };

  // fetch user data
  useEffect(() => {
    const FetchData = async () => {
      try {
        const response = await axios.get('https://ghablameh.fiust.ir/api/v1/organizations/me/', {
          headers: {
            'Authorization': 'JWT ' + localStorage.getItem("token")
          }
        });

        if (response.status === 200) {
          setName(response.data.name || '');
          setAdmin_first_name(response.data.admin_first_name || '');
          setAdmin_last_name(response.data.admin_last_name || '');
          SetAdmin_username(response.data.admin_username || '');
          setAdmin_email(response.data.admin_email || '');
          setAdmin_phone_number(response.data.admin_phone_number || '');
          setImage_url(response.data.image_url || '')

        } else {
          console.log('fetching userData failed:', response.status);
        }
      } catch (error) {
        console.error('An error occurred:', error);
      }

    };
    FetchData();
  }, []);

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

  //form
  function Form() {
    return (
      <div className={styles.container} >
        <div className='flex justify-center items-center'>
          <div className={styles.card}>
            <h2 className={styles.title}>به‌روزرسانی اطلاعات سازمان </h2>

            {/* info */}
            <form onSubmit={handleFormSubmit} className={`${styles.form}`}>
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
                            name={admin_first_name}
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

              {/* input fileds */}
              <div>
                <div className={`${styles.formGroup} mt-3`}>
                  <label htmlFor="name" className={styles.label}> نام سازمان  </label>
                  <input type="text" id="name" style={{ borderRadius: '10px' }} value={name} onChange={(e) => setName(e.target.value)} className={styles.input} placeholder='نام سازمان' />
                </div>

                <div className={styles.formGroup}>
                  <div className='flex mt-8'>
                    <div className='w-1/2'>
                      <label htmlFor="adminFirstName" className={styles.label}>نام مدیر سازمان</label>
                      <input
                        type="text"
                        id="adminFirstName"
                        style={{ borderRadius: '10px' }}
                        value={admin_first_name}
                        onChange={(e) => setAdmin_first_name(e.target.value)}
                        className={styles.input}
                        placeholder='نام'
                      />
                    </div>
                    <div className='w-1/2 mr-4'>
                      <label htmlFor="adminLastName" className={styles.label}>نام خانوادگی مدیر سازمان</label>
                      <input
                        type="text"
                        id="adminLastName"
                        style={{ borderRadius: '10px' }}
                        value={admin_last_name}
                        onChange={(e) => setAdmin_last_name(e.target.value)}
                        className={styles.input}
                        placeholder='نام خانوادگی'
                      />
                    </div>
                  </div>
                </div>

                <div className={styles.formGroup}>
                  <div className='flex mt-8'>
                    <div className='w-1/2'>
                      <label htmlFor="admin_username" className={styles.label}> نام کاربری مدیر سازمان  </label>
                      <input type="text" id="admin_username" style={{ borderRadius: '10px', direction: 'ltr' }} value={admin_username} onChange={(e) => SetAdmin_username(e.target.value)} className={styles.input} placeholder=' نام کاربری مدیر سازمان  ' />

                    </div>
                    <div className='w-1/2 mr-4'>
                      <label htmlFor="admin_phone_number" className={styles.label}> شماره تماس مدیر سازمان </label>
                      <input type="text" id="admin_phone_number" style={{ borderRadius: '10px', direction: 'ltr' }} value={admin_phone_number} onChange={(e) => setAdmin_phone_number(e.target.value)} className={styles.input} placeholder=' شماره تماس مدیر سازمان ' />
                    </div>
                  </div>
                </div>


                <div className={styles.formGroup}>
                  <label htmlFor="admin_email" className={styles.label}> ایمیل مدیر سازمان </label>
                  <input type="text" id="admin_email" style={{ borderRadius: '10px', direction: 'ltr' }} value={admin_email} onChange={(e) => setAdmin_email(e.target.value)} className={styles.input} placeholder='  ایمیل مدیر سازمان  ' />
                </div>

                {/* submit button */}
                {!isWaitingForm1 && (
                  <button type="submit" className={styles.button} style={{ borderRadius: '10px' }}>
                    ارسال اطلاعات
                  </button>
                )}
                {isWaitingForm1 && (
                  <button type="submit" className={styles.button} style={{ borderRadius: '10px' }}>
                    <div className={`${styles.spinner2}`}></div>
                  </button>
                )}

              </div>

            </form>

            {/* pass */}
            <form onSubmit={handlePassSubmit} className={`${styles.form}`}>

              {/* password title */}
              <div className='m-3'>
                <hr style={{ borderTop: '1px solid rgb(38, 87, 124)', marginBottom: '20px', marginTop: '40px' }} />
                <h2 className={styles.title}>به‌روزرسانی رمز عبور </h2>
              </div>

              {/* password fields */}
              <div>
                <div className={styles.formGroup}>
                  <label htmlFor="currentPassword" className={styles.label}>
                    رمز عبور فعلی
                  </label>
                  <div className={styles.passwordInputContainer}>
                    <input
                      type={showCurrentPassword ? 'text' : 'password'}
                      id="old_password"
                      value={old_password}
                      onChange={(e) => setOld_password(e.target.value)}
                      className={styles.input}
                      style={{ borderRadius: '10px' }}
                    // required
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
                      id="new_password"
                      value={new_password}
                      onChange={(e) => setNew_password(e.target.value)}
                      className={styles.input}
                      style={{ borderRadius: '10px' }}
                    // required
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
                      id="confirm_new_password"
                      value={confirm_new_password}
                      onChange={(e) => setConfirm_new_password(e.target.value)}
                      className={styles.input}
                      style={{ borderRadius: '10px' }}
                    // required
                    />
                    <FontAwesomeIcon
                      icon={showConfirmPassword ? faEye : faEyeSlash}
                      className={styles.passwordIcon}
                      onClick={toggleConfirmPasswordVisibility}
                    />
                  </div>
                </div>
              </div>

              {/* submit button */}
              {!isWaitingForm2 && (
                <button type="submit" className={styles.button} style={{ borderRadius: '10px' }}>
                  به روز رسانی رمز عبور
                </button>
              )}
              {isWaitingForm2 && (
                <button type="submit" className={styles.button} style={{ borderRadius: '10px' }}>
                  <div className={`${styles.spinner2}`}></div>
                </button>
              )}
            </form>

          </div>
        </div>
      </div>
    );
  }

  //send form info  
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


    if (!name) {
      errors.push('نام سازمان را وارد کنید.');

      createNotification('fail', 'نام سازمان را وارد کنید.')();
    }
    if (!admin_first_name) {
      errors.push('نام مدیر را وارد کنید.');

      createNotification('fail', 'نام مدیر را وارد کنید.')();
    }
    if (!admin_last_name) {
      errors.push('نام خانودگی مدیر را وارد کنید.');

      createNotification('fail', 'نام خانودگی مدیر را وارد کنید.')();
    }
    if (!admin_username) {
      errors.push('نام کاربری مدیر را وارد کنید.');

      createNotification('fail', 'نام کاربری مدیر را وارد کنید.')();
    }
    if (!admin_email) {
      errors.push(' ایمیل مدیر را وارد کنید.');

      createNotification('fail', ' ایمیل مدیر را وارد کنید.')();
    }
    if (!admin_phone_number) {
      errors.push('شماره مدیر را وارد کنید.');
      createNotification('fail', 'شماره مدیر را وارد کنید.')();
    }
    if (!/^([a-zA-Z0-9!_.]+)@([a-zA-Z]+)\.([a-zA-Z]{2,})$/.test(admin_email)) {
      errors.push('ایمیل مدیر را به درستی وارد کنید.');
      createNotification('fail', 'ایمیل مدیر را به درستی وارد کنید.')();
    }
    if (admin_phone_number.startsWith('98') && admin_phone_number.length !== 12) {
      errors.push('شماره مدیر را به درستی وارد کنید.');
      createNotification('fail', 'شماره مدیر را به درستی وارد کنید.')();
    }
    if (admin_phone_number.startsWith('09') && admin_phone_number.length !== 11) {
      errors.push('شماره مدیر را به درستی وارد کنید.');
      createNotification('fail', 'شماره مدیر را به درستی وارد کنید.')();
    }

    let admin_phone_number_english = admin_phone_number.replace(/[۰-۹]/g, function (w) {
      return engNumbers[w]
    });

    // check if admin_phone_number contains only numbers and starts with '989' or '09'
    if (!/^\d+$/.test(admin_phone_number_english) || !/^(989|09)/.test(admin_phone_number_english)) {
      errors.push('شماره مدیر را به درستی وارد کنید.');
      createNotification('fail', 'شماره مدیر را به درستی وارد کنید.')();
    }

    if (errors.length > 0) {
      // alert(errors.join('\n'));

      setFormErrors(errors);
      return;
    }

    //form data
    const formData = {
      // image_base64:image_base64 || '',
      name,
      admin_first_name,
      admin_last_name,
      admin_username,
      admin_email,
      admin_phone_number: admin_phone_number_english
    };

    if (image_base64) {
      formData.image_base64 = image_base64;
    }

    //send form data
    setIsWaitingForm1(true);
    try {
      const response = await axios.put('https://ghablameh.fiust.ir/api/v1/organizations/me/', formData,
        { headers: { Authorization: "JWT " + localStorage.getItem("token") } }
      );

      if (response.status === 200) {
        setIsWaitingForm1(false);
        // alert('اطلاعات با موفقیت ثبت شد ');
        // window.location.href = '/';
        console.log('428',response);

        createNotification('success')();
      } else {
        const errorData = await response.json();
        setIsWaitingForm1(false);
        console.log('434',response);
        createNotification('fail', response.message)();
      }
    } catch (error) {
      setIsWaitingForm1(false);
      console.error('439',error);

      if (error.response.data.name) {
        // alert(error.response.data.name);
        createNotification('fail', error.response.data.name)();
        return;
      }
      if (error.response.data.admin_first_name) {
        // alert(error.response.data.admin_first_name);
        createNotification('fail', error.response.data.admin_first_name)();
        return;
      }
      if (error.response.data.admin_last_name) {
        // alert(error.response.data.admin_last_name);
        createNotification('fail', error.response.data.admin_last_name)();
        return;
      }
      if (error.response.data.admin_username) {
        // alert(error.response.data.admin_username);
        createNotification('fail', error.response.data.admin_username)();
        return;
      }
      if (error.response.data.admin_email) {
        // alert(error.response.data.admin_email);
        createNotification('fail', error.response.data.admin_email)();
        return;
      }
      if (error.response.data.admin_phone_number) {
        // alert(error.response.data.admin_phone_number);
        createNotification('fail', error.response.data.admin_phone_number)();
        return;
      }
      createNotification('fail',' مشکلی پیش آمده، در زمان دیگری امتحان کنید ')();
    }
    setIsWaitingForm1(false);
    
  };

  //send form pass
  const handlePassSubmit = async (e) => {
    e.preventDefault();

    const errors = [];
    setPassErrors('');

    if (!new_password) {
      errors.push('رمز عبور جدید را وارد کنید.');
      createNotification('fail', 'رمز عبور جدید را وارد کنید.')();

    }
    if (!old_password) {
      errors.push('رمز عبور فعلی را وارد کنید.');
      createNotification('fail', 'رمز عبور فعلی را وارد کنید')();

    }
    if (!confirm_new_password) {
      errors.push('تأیید رمز عبور جدید را وارد کنید.');
      createNotification('fail', 'تأیید رمز عبور جدید را وارد کنید.')();
    }
    if (new_password !== confirm_new_password) {
      errors.push('رمز عبور جدید و تأیید آن مطابقت ندارند.');
      createNotification('fail', 'رمز عبور جدید و تأیید آن مطابقت ندارند.')();
    }
    if (!/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(new_password)) {
      errors.push(' رمز عبور باید حداقل ۸ کاراکتر و شامل اعداد و حروف باشد. ');
      createNotification('fail', ' رمز عبور باید حداقل ۸ کاراکتر و شامل اعداد و حروف باشد. ')();
    }

    if (errors.length > 0) {
      // setPassErrors(errors);
      // alert(errors.join('\n'));
      return;
    }

    // Pass data for password update
    const passData = {
      old_password: old_password,
      new_password: new_password,
      confirmPassword: confirm_new_password
    };

    // send pass data
    setIsWaitingForm2(true);
    try {
      const response = await axios.post('https://ghablameh.fiust.ir/api/v1/organizations/password/', passData,
        { headers: { Authorization: "JWT " + localStorage.getItem("token") } }
      );

      if (response.status === 200) {
        setIsWaitingForm2(false);
        // alert('اطلاعات با موفقیت ثبت شد ');
        // window.location.href = '/';
        createNotification('success')();
      } else {
        const errorData = await response.json();
        setIsWaitingForm2(false);
        // alert(' مشکلی پیش امده.لطفا در زمانی دیگر امتحان کنید ')
        createNotification('fail', ' مشکلی پیش امده.لطفا در زمانی دیگر امتحان کنید ')();
      }
    } catch (error) {
      // console.error('An error occurred:', error);
      setIsWaitingForm2(false);
      if (error.response.data.old_password) {
        // alert(error.response.data.old_password[0]);
        createNotification('fail', error.response.data.old_password[0])();
      } else if (error.response.data.new_password) {
        // alert(error.response.data.new_password[0]);
        createNotification('fail', error.response.data.new_password[0])();
      } else if (error.response.data.new_password & error.response.data.old_password) {
        // alert(error.response.data.new_password[0]);
        // alert(error.response.data.old_password[0]);
        createNotification('fail', error.response.data.new_password[0])();
        createNotification('fail', error.response.data.old_password[0])();
      }
    }
    setIsWaitingForm2(false);
  };

  return (
    <div className={`${styles.bg} flex flex-col`} >
      <Navbarparent />
      <NotificationContainer />
      {Form()}
    </div>
  );
};

export default Update;