import React, { useState , useEffect} from 'react';
import axios from 'axios';
import styles from '../styles/org.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const [email, setEmail] = useState('');
  const [username, setUsername] = useState('');
  const [organization_name, setOrganizationName] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [first_name, setFirstName] = useState('');
  const [last_name, setLastName] = useState('');
  const [phone_number, setPhoneNumber] = useState('');
  const navigate = useNavigate();
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('https://ghablameh.fiust.ir/api/v1/swagger/?format=openapi#/definitions/OrganizationAdminCreate');
        console.log(response.data);
        
      } catch (error) {
        console.log('Error while fetching data:', error);
      }
    };

    fetchData();
  }, []);
  const togglePasswordVisibility = () => {
    setShowPassword((prevState) => !prevState);
  };

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleUsernameChange = (e) => {
    setUsername(e.target.value);
  };

  const handleOrganizationNameChange = (e) => {
    setOrganizationName(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };

  const handleFirstNameChange = (e) => {
    setFirstName(e.target.value);
  };

  const handleLastNameChange = (e) => {
    setLastName(e.target.value);
  };

  const handlePhoneNumberChange = (e) => {
    setPhoneNumber(e.target.value);
  };

  const handleSubmit = async (e) => {
  e.preventDefault();

  const userData = {
    organization_name,
    username,
    password,
    email,
    first_name,
    last_name,
    phone_number,
  };

  try {
    // const response = await axios.post(
    //   'https://ghablameh.fiust.ir/api/v1/organizations/register/',
    //   userData,
    //   {
    //     headers: {
    //       'Content-Type': 'application/json',
    //       'X-CSRFToken': '4IqnkAsVtRhkrwE8YiGnyiQFkbvCrIJRrFjxMcqXAmLBESd8MCuulfCFSHFSTpIr',
    //     },
    //   }
    //   userData
    // );
    const response = await axios.post('https://ghablameh.fiust.ir/api/v1/organizations/register/', userData);
      const accessToken = response.data.tokens.access;
      localStorage.setItem('token', accessToken);
      navigate('/')
    console.log('Registration successful');
    setEmail('');
    setUsername('');
    setOrganizationName('');
    setPassword('');
    setFirstName('');
    setLastName('');
    setPhoneNumber('');
  } catch (error) {
    console.log('Error during registration:', error);
  }
};
  return (
    <div className={styles.container}>
      <div className={styles.pattern}></div>
      <div className={styles.card}>
        <h2 className={styles.title}>ثبت نام سازمان</h2>
        <form onSubmit={handleSubmit}>
          <div className={styles.formGroup}>
            <label htmlFor="email" className={styles.label}>
              ایمیل
            </label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={handleEmailChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="username" className={styles.label}>
              نام کاربری مدیر
            </label>
            <input
              type="text"
              id="username"
              value={username}
              onChange={handleUsernameChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="organizationName" className={styles.label}>
              نام سازمان
            </label>
            <input
              type="text"
              id="organizationName"
              value={organization_name}
              onChange={handleOrganizationNameChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="organizationName" className={styles.label}>
              شماره تلفن
            </label>
            <input
              type="text"
              id="organizationName"
              value={organizationName}
              onChange={handleOrganizationNameChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="organizationName" className={styles.label}>
              نام ادمین
            </label>
            <input
              type="text"
              id="organizationName"
              value={organizationName}
              onChange={handleOrganizationNameChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="organizationName" className={styles.label}>
               نام خانوادکی ادمین
            </label>
            <input
              type="text"
              id="organizationName"
              value={organization_name}
              onChange={handleOrganizationNameChange}
              className={styles.input}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="currentPassword" className={styles.label}>
              رمز عبور
            </label>
            <div className={styles.passwordInputContainer}>
              <input
                type={showPassword ? 'text' : 'password'}
                id="currentPassword"
                value={password}
                onChange={handlePasswordChange}
                className={styles.input}
                required
              />
              <FontAwesomeIcon
                icon={showPassword ? faEyeSlash : faEye}
                className={styles.passwordIcon}
                onClick={togglePasswordVisibility}
              />
            </div>
    </div>
          <div className={styles.formGroup}>
            <label htmlFor="firstName" className={styles.label}>
             نام ادمین
            </label>
            <input
              type="text"
              id="firstName"
              value={first_name}
              onChange={handleFirstNameChange}
              className={styles.input}
            />
          </div>
     
          <div className={styles.formGroup}>
            <label htmlFor="lastName" className={styles.label}>
              نام خانوادگی ادمین 
            </label>
            <input
              type="text"
              id="lastName"
              value={last_name}
              onChange={handleLastNameChange}
              className={styles.input}
            />
            <div className={styles.formGroup}>
              <label htmlFor="phoneNumber" className={styles.label}>
                شماره تلفن ادمین
              </label>
              <input
                type="text"
                id="phoneNumber"
                value={phone_number}
                onChange={handlePhoneNumberChange}
                className={styles.input}
              />
            </div>
            <button type="submit" className={styles.button}>
              ثبت نام
            </button>
      
            </div>
          </form>
        </div>
      </div>
    );
  }
  
  export default Register;