import React, { useState } from 'react';
import logo from '/password22.png';
import styles from '../listorg/home.module.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';

const ForgetPasswordWindow = ({ onClose }) => {
  const [email, setEmail] = useState('');
  const [code, setCode] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [newPassword2, setNewPassword2] = useState('');
  const [message, setMessage] = useState('');
  const [formErrors, setFormErrors] = useState('');
  const [stage, setStage] = useState(1); // 1: forget password, 2: verify code, 3: change password

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleCodeChange = (e) => {
    setCode(e.target.value);
  };

  const handleNewPasswordChange = (e) => {
    setNewPassword(e.target.value);
  };

  const handleNewPassword2Change = (e) => {
    setNewPassword2(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    setFormErrors('');
    const errors = [];

    if (stage === 1) {
      // Forget password stage
      if (!email) {
        setFormErrors('ایمیل را وارد کنید.');
        return;
      }
      if (!/^([a-zA-Z0-9!_.]+)@([a-zA-Z]+)\.([a-zA-Z]{2,})$/.test(email)) {
        setFormErrors('ایمیل را به درستی وارد کنید.');
        return;
      }
      if (errors.length > 0) {
        setFormErrors(errors);
        return;
      }

      try {
        const response = await axios.post('https://ghablameh.fiust.ir/api/v1/verification/forget_password/', { email });
        console.log("Response data:", response.data);
        alert('درخواست با موفقیت ارسال شد.');
        setStage(2);
      } catch (error) {
        console.error("Error occurred:", error);
        alert('درخواست با مشکل مواجه شد. لطفا در زمان دیگری مجددا امتحان کنید.');
      }
    } else if (stage === 2) {
      // Verify code stage
      if (!code || !email || !newPassword || !newPassword2) {
        setFormErrors('تمام فیلدها را پر کنید.');
        return;
      }
      if (newPassword !== newPassword2) {
        setFormErrors('رمزهای عبور یکسان نیستند.');
        return;
      }

      try {
        const data =
        {
          code,
          email,
          new_password: newPassword,
          new_password2: newPassword2,
        } ;
        console.log("Response data:", data);

        const response = await axios.post('https://ghablameh.fiust.ir/api/v1/verification/forget_password_verify/', {
          code,
          email,
          new_password: newPassword,
          new_password2: newPassword2,
        });
       
        alert('رمز عبور با موفقیت تغییر یافت.');
        onClose();
      } catch (error) {
        console.error("Error occurred:", error);
        alert('درخواست با مشکل مواجه شد. لطفا در زمان دیگری مجددا امتحان کنید.');
      }
    }
  };

  return (
    <div className={`${styles['forget-password-window']} flex flex-col items-center justify-center`}>
      <h2 className='font-bold text-9xl mb-14'>بازیابی رمز عبور</h2>
      <div className={`${styles.icon} mt-7 flex flex-col items-center justify-center`}>
        <img src={logo} alt="Icon" />
      </div>
      <span className={styles['close-icon']} onClick={onClose}>
        <FontAwesomeIcon icon={faTimes} />
      </span>

      {stage === 1 && (
        <div className='mt-6 mb-2 flex flex-col items-center justify-center'>
          <h3 className='text-sm font-medium'>ایمیل خود را وارد نمایید.</h3>
          <form onSubmit={handleSubmit} className='flex flex-col items-center justify-center'>
            <div className='flex flex-col items-center justify-center my-1'>
              <input
                type="email"
                placeholder="ایمیل"
                value={email}
                onChange={handleEmailChange}
                className="w-80 p-2 rounded-xl mb-2"
                style={{ backgroundColor: 'white', border: '1px solid rgb(38, 87, 124' }}
                required
              />
              <div>{formErrors && <span className='text-red-600 text-sm'>{formErrors}</span>}</div>
            </div>
            <button type="submit" className='w-60 mt-3'>بازیابی رمز عبور</button>
            {message && <h3 className='text-sm font-medium mt-2 text-red-500'>{message}</h3>}
          </form>
        </div>
      )}

      {stage === 2 && (
        <div className='mt-6 mb-2 flex flex-col items-center justify-center'>
          <h3 className='text-sm font-medium'>کد ارسال شده به ایمیل را وارد کنید و رمز عبور جدید را تعیین نمایید.</h3>
          <form onSubmit={handleSubmit} className='flex flex-col items-center justify-center'>
            <div className='flex flex-col items-center justify-center my-1'>
              <input
                type="text"
                placeholder="کد"
                value={code}
                onChange={handleCodeChange}
                className="w-80 p-2 rounded-xl mb-2"
                style={{ backgroundColor: 'white', border: '1px solid rgb(38, 87, 124' }}
                required
              />
              <input
                type="password"
                placeholder="رمز عبور جدید"
                value={newPassword}
                onChange={handleNewPasswordChange}
                className="w-80 p-2 rounded-xl mb-2"
                style={{ backgroundColor: 'white', border: '1px solid rgb(38, 87, 124' }}
                required
              />
              <input
                type="password"
                placeholder="تکرار رمز عبور جدید"
                value={newPassword2}
                onChange={handleNewPassword2Change}
                className="w-80 p-2 rounded-xl mb-2"
                style={{ backgroundColor: 'white', border: '1px solid rgb(38, 87, 124' }}
                required
              />
              <div>{formErrors && <span className='text-red-600 text-sm'>{formErrors}</span>}</div>
            </div>
            <button type="submit" className='w-60 mt-3'>تغییر رمز عبور</button>
            {message && <h3 className='text-sm font-medium mt-2 text-red-500'>{message}</h3>}
          </form>
        </div>
      )}
    </div>
  );
};

export default ForgetPasswordWindow;