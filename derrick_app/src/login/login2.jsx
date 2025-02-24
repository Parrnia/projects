import React, { useState } from 'react';
import styles from './login2.module.css';
import ReCAPTCHA from 'react-google-recaptcha';
import { useNavigate } from 'react-router-dom';
import { useRef } from 'react';
import * as Yup from 'yup';
const validationSchema = Yup.object().shape({
  firstname: Yup.string().required('نام اجباری است'),
  lastName: Yup.string().required('نام خانوادگی اجباری است'),
  id: Yup.string()
    .required('کد ملی اجباری است')
    .matches(/^\d{10}$/, 'کد ملی نامعتبر است'),
  number: Yup.string()
    .required('تلفن همراه اجباری است')
    .matches(/^\d{11}$/, 'شماره تلفن نامعتبر است'),
  rep_id: Yup.string()
    .required('کد ملی معرف اجباری است')
    .matches(/^\d{10}$/, 'کد ملی معرف نامعتبر است'),
  log_num: Yup.string()
    .required('تلفن همراه اجباری است')
    .matches(/^\d{11}$/, 'شماره تلفن نامعتبر است'),
  log_pass: Yup.string()
    .required('رمز عبور اجباری است')
    .min(8, 'رمز عبور باید حداقل 8 کاراکتر باشد')
    .matches(/[a-z]/, 'رمز عبور باید شامل حروف کوچک باشد')
    .matches(/[A-Z]/, 'رمز عبور باید شامل حروف بزرگ باشد')
    .matches(/\d/, 'رمز عبور باید شامل عدد باشد')
    .matches(/[!@#$%^&*(),.?":{}|<>]/, 'رمز عبور باید شامل کاراکتر خاص باشد'),
  bussinesname: Yup.string().required('نام کسب و کار اجباری است'),
  bussinesid: Yup.string().required('شناسه ملی اجباری است'),
  Registrationnumber: Yup.string().required('شماره ثبت اجباری است'),
  companyname: Yup.string().required('نام فروشگاه اجباری است'),
});

const Login2 = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate('/log'); 
  };
  const handleArrowClick = () => {
    navigate(-1); 
  };

  const [capVal, setCapVal] = useState(null);
  const [logcapVal, setLogCapVal] = useState(null);
  const [values, setValues] = useState({
    firstname: '',
    lastName: '',
    bussinesname: '',
    bussinesid: '',
    Registrationnumber: '',
    companyname: '',
    id: '',
    number: '',
    desc: '',
    rep_id: '',
  });
  const [logValues, setLogValues] = useState({
    log_num: '',
    log_pass: ''
  });
  const [errors, setErrors] = useState({});
  const [logerrors, setLogErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
  };

  const loghandleChange = (e) => {
    const { name, value } = e.target;
    setLogValues({ ...logValues, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await validationSchema.validate(values, { abortEarly: false });
      if (capVal) {
        // Handle form submission
      }
    } catch (err) {
      const newErrors = {};
      err.inner.forEach((error) => {
        newErrors[error.path] = error.message;
      });
      setErrors(newErrors);
    }
  };

  const loghandleSubmit = async (e) => {
    e.preventDefault();
    try {
      await validationSchema.validate(logValues, { abortEarly: false });
      if (logcapVal) {
        // Handle form submission
      }
    } catch (err) {
      const newErrors = {};
      err.inner.forEach((error) => {
        newErrors[error.path] = error.message;
      });
      setLogErrors(newErrors);
    }
  };
  const captchaRef = useRef(null)


  return (


    <div>
    <div className={styles.background}>
      <div className={styles.box}>
        <div className={styles.logo}></div>
        <p>ایجاد حساب کاربری</p>
        <div className={styles['form-container']}>
          <div className={styles.form}>
            {errors.bussinesname && <div className={styles.error}>{errors.bussinesname}</div>}
            <input
              type="text"
              name="bussinesname"
              value={values.bussinesname}
              onChange={handleChange}
              placeholder="نام کسب و کار"
            />
            {errors.Registrationnumber && <div className={styles.error}>{errors.Registrationnumber}</div>}
            <input
              type="text"
              name="Registrationnumber"
              value={values.Registrationnumber}
              onChange={handleChange}
              placeholder="شماره ثبت"
            />
            {errors.bussinesid && <div className={styles.error}>{errors.bussinesid}</div>}
            <input
              type="text"
              name="bussinesid"
              value={values.bussinesid}
              onChange={handleChange}
              placeholder="شناسه ملی"
            />
            {errors.companyname && <div className={styles.error}>{errors.companyname}</div>}
            <input
              type="text"
              name="companyname"
              value={values.companyname}
              onChange={handleChange}
              placeholder="نام فروشگاه"
            />
            {errors.rep_id && <div className={styles.error}>{errors.rep_id}</div>}
            <input
              type="text"
              name="rep_id"
              value={values.rep_id}
              onChange={handleChange}
              placeholder="کد ملی معرف"
            />
          </div>
          <div className={styles.form}>
            {errors.firstname && <div className={styles.error}>{errors.firstname}</div>}
            <input
              type="text"
              name="firstname"
              value={values.firstname}
              onChange={handleChange}
              placeholder="نام مدیر عامل"
            />
            {errors.lastName && <div className={styles.error}>{errors.lastName}</div>}
            <input
              type="text"
              name="lastName"
              value={values.lastName}
              onChange={handleChange}
              placeholder="نام خانوادگی مدیر عامل"
            />
            {errors.id && <div className={styles.error}>{errors.id}</div>}
            <input
              type="text"
              name="id"
              value={values.id}
              onChange={handleChange}
              placeholder="کدملی مدیر عامل"
            />
            {errors.number && <div className={styles.error}>{errors.number}</div>}
            <input
              type="tel"
              name="number"
              value={values.number}
              onChange={handleChange}
              placeholder="تلفن همراه"
            />
            {errors.desc && <div className={styles.error}>{errors.desc}</div>}
            <input
              type="text"
              name="desc"
              value={values.desc}
              onChange={handleChange}
              placeholder="توضیحات"
            />
          </div>
        </div>
        <div>
      <div className={styles.enum1} onClick={handleClick}></div>
    </div>
        <div className={styles.rec}>
          <ReCAPTCHA
            sitekey="6LcddlUqAAAAAEn6_ivwd7R65bq09MBCsSvUCsav"
             ref={captchaRef}
            onChange={(val) => setCapVal(val)}
          />
    
        </div>
        <button className={styles['submit-btn']} onClick={handleSubmit} disabled={!capVal}>
          ثبت نام
        </button>
      </div>
      <div className={styles.box1}>
        <div className={styles.logo}></div>
        <p>ورود به حساب کاربری</p>
        <div className={styles['login-form-container']}>
          <div className={styles['login-form']}>
            {logerrors.log_num && <div className={styles.logerror}>{logerrors.log_num}</div>}
            <input
              type="tel"
              name="log_num"
              value={logValues.log_num}
              onChange={loghandleChange}
              placeholder="تلفن همراه"
            />
            {logerrors.log_pass && <div className={styles.logerror}>{logerrors.log_pass}</div>}
            <input
              type="password"
              name="log_pass"
              value={logValues.log_pass}
              onChange={loghandleChange}
              placeholder="گذرواژه"
            />
          </div>
        </div>
        <div className={styles['log-rec']}>
          <ReCAPTCHA
            sitekey="6Le7eVUqAAAAABzIvY29P2SrtuXYEm9UZbuQ7Ewo"
            onChange={(val) => setLogCapVal(val)}
          />
        </div>
        <button className={styles['login-btn']} onClick={loghandleSubmit} disabled={!logcapVal}>
          ورود
        </button>
        <div className={styles.path}></div>
      </div>
      <div className={styles['arrow-up']}></div>
      <div className={styles.arrow}  onClick={handleArrowClick} ></div>
    </div>
  </div>
  );
};

export default Login2;