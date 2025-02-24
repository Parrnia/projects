import React, { useEffect, useRef, useState } from 'react';
import styles from './login.module.css';
import * as Yup from 'yup';
import { useNavigate } from 'react-router-dom';
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
    .matches(/[!@#$%^&*(),.?":{}|<>]/, 'رمز عبور باید شامل کاراکتر خاص باشد')
});
const validationSchemalog = Yup.object().shape({
  log_num: Yup.string()
    .required('تلفن همراه اجباری است')
    .matches(/^\d{11}$/, 'شماره تلفن نامعتبر است'),
  log_pass: Yup.string()
    .required('رمز عبور اجباری است')
    .min(8, 'رمز عبور باید حداقل 8 کاراکتر باشد')
    .matches(/[a-z]/, 'رمز عبور باید شامل حروف کوچک باشد')
    .matches(/[A-Z]/, 'رمز عبور باید شامل حروف بزرگ باشد')
    .matches(/\d/, 'رمز عبور باید شامل عدد باشد')
    .matches(/[!@#$%^&*(),.?":{}|<>]/, 'رمز عبور باید شامل کاراکتر خاص باشد')
});

const ReCAPTCHAManual = ({ sitekey, onChange }) => {
  const recaptchaRef = useRef(null);

  useEffect(() => {
    const loadScript = async () => {
      const script = document.createElement('script');
      script.src = 'https://www.google.com/recaptcha/api.js?render=explicit';
      script.async = true;
      script.defer = true;
      document.body.appendChild(script);

      const grecaptcha = await window.grecaptcha;
      const recaptcha = await grecaptcha.render(recaptchaRef.current, {
        sitekey,
        callback: onChange,
      });

      return () => grecaptcha.reset(recaptcha);
    };

    loadScript();
  }, [sitekey, onChange]);

  return <div ref={recaptchaRef} />;
};

const Login = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate('/log2'); 
  };
  const handleArrowClick = () => {
    navigate(-1); 
  };

  const [capVal, setCapVal] = useState(null);
  const [logcapVal, setLogCapVal] = useState(null);
  const [values, setValues] = useState({
    firstname: '',
    lastName: '',
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
  const [logerrors, setlogErrors] = useState({});
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
      await validationSchemalog.validate(logValues, { abortEarly: false });
      if (logcapVal) {
        // Handle form submission
      }
    } catch (err) {
      const newErrors = {};
      err.inner.forEach((error) => {
        newErrors[error.path] = error.message;
      });
      setlogErrors(newErrors);
    }
  };

  return (
    <div>
      <div className={styles.background}>
      <div className={styles.box}>
        <div className={styles.logo}></div>
        <p>ایجاد حساب کاربری</p>
        <div className={styles['form-container']}>
          <div className={styles.form}>
            {errors.firstname && <div className={styles.error}>{errors.firstname}</div>}
            <input
              type="text"
              name="firstname"
              value={values.firstname}
              onChange={handleChange}
              placeholder="نام"
            />
            {errors.lastName && <div className={styles.error}>{errors.lastName}</div>}
            <input
              type="text"
              name="lastName"
              value={values.lastName}
              onChange={handleChange}
              placeholder="نام خانوادگی"
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
            {errors.number && <div className={styles.error}>{errors.number}</div>}
            <input
              type="tel"
              name="number"
              value={values.number}
              onChange={handleChange}
              placeholder="تلفن همراه"
            />
            {errors.id && <div className={styles.error}>{errors.id}</div>}
            <input
              type="text"
              name="id"
              value={values.id}
              onChange={handleChange}
              placeholder="کد ملی"
            />
            <input
              type="text"
              name="desc"
              value={values.desc}
              onChange={handleChange}
              placeholder="توضیحات"
            />
          </div>
        </div>
        <div className={styles.enum}  onClick={handleClick}  ></div>
        <div className={styles.rec}>
          <ReCAPTCHAManual
            sitekey="6LdEf1UqAAAAACbfmFwK4z_sB5WUpDfCB2C8rZUb"
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
            {logerrors.log_pass && <div className={styles.logerror} >{logerrors.log_pass}</div>}
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
          <ReCAPTCHAManual
            sitekey="6LfCf1UqAAAAACHZ6HbJAltai5SfqEcXIly5FZEH"
            onChange={(val) => setLogCapVal(val)}
          />
        </div>
        <button className={styles['login-btn']}  onClick={loghandleSubmit} disabled={!logcapVal}>
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

export default Login;