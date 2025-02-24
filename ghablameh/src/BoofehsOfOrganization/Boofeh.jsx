import React, { useState } from 'react';
import styles from "./Boofeh.module.css";
import axios from 'axios';
function Boofeh({ searchTerm, onSearchChange }) {
  //model
  const [showModel, setShowModel] = useState(false);
  const [submitError, setSubmitError] = useState('');
  const onClose = () => {
    setShowModel(false);
    setSubmitError('');
  };

  const handleOnClose = (e) => {
    if (e.target.id === "close") onClose();
  };

  //data for backend
  const [buffetData, setBuffetData] = useState({
    name: '',
  });
  const handleAddBuffet = (event) => {
    setBuffetData({ ...buffetData, [event.target.name]: event.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Retrieve token
    const token = 'JWT ' + localStorage.getItem("token");
    // const token='JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzE2MTM2NjEzLCJpYXQiOjE3MTM1NDQ2MTMsImp0aSI6IjBiN2YwNzM4OWMxYTRmMzZiNWM1MDQ2ZDhhM2NmOWE1IiwidXNlcl9pZCI6MzF9.IotlHGPBqWO2tSWq50Q5cD87JNrbyDTqt6HbA91rShg'

    // Send form data
    try {
      const response = await axios.post('https://ghablameh.fiust.ir/api/v1/buffets/', buffetData, {
        headers: {
          Authorization: token,
        }
      });
      // console.log(response.status)
      if (response.status === 201) {
        setShowModel(false);
      } else {
        const errorData = response.data;
  
        if (response.status === 400 && errorData.name && errorData.name[0] === "This field may not be blank.") {
          setSubmitError('نام بوفه نمی تواند خالی باشد');
        } else {
          setSubmitError('نام بوفه نمی تواند خالی باشد');
        }
      }
    } catch (error) {
      // console.error('An error occurred:', error);
      setSubmitError('نام بوفه نمی تواند خالی باشد');
    }
  };


  return (
    <>
      <div className='flex flex-col'>
        <form className="mt-52 max-w-md mx-auto items-center">
          <div>
            <div className="relative">
              <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                <svg className="w-4 h-4 text-template-custom-white dark:text-template-custom-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                  <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
                </svg>
              </div>
              <input
                type="search"
                id="default-search"
                className="mb-10 block w-full p-4 ps-10 text-sm text-gray-1 border border-gray-500 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-template-custom-orange focus:border-transparent placeholder-template-custom-gray text-gray-500 dark:bg-template-custom-gray dark:border-template-custom-blue dark:placeholder-white dark:text-white dark:focus:ring-2 dark:focus:border-transparent"
                placeholder=" جست و جو بوفه... "
                value={searchTerm}
                onChange={onSearchChange}
              />
            </div>
          </div>
        </form>
      </div>

    </>
  );
}

export default Boofeh;