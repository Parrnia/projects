import { useState, useEffect } from 'react';
import { XIcon, CheckIcon } from '@heroicons/react/solid';
import Avatar from 'react-avatar';
import styles from '../styles/listofrequests.module.css';
import axios from 'axios';
import 'react-toastify/dist/ReactToastify.css';
import useMediaQuery from '@mui/material/useMediaQuery';
import Navbarparent from './navbarparent';

function ListOfJoinRequests() {
  const [IsSelectedUser, setIsSelectedUser] = useState(false)
  const isBigScreen = useMediaQuery('(min-width: 650px)')
  const [requests, setRequests] = useState([]);

  //fetch list of requests
  const token = 'JWT ' + localStorage.getItem("token");

  //fetchUserData
  const fetchRequests = async () => {
    try {
      const response = await axios.get('https://ghablameh.fiust.ir/api/v1/organizations/join-requests/', {
        headers: {
          'Authorization': token
        }
      });
      const pendingRequests = response.data.filter((request) => request.status === "P");

      setRequests(pendingRequests);
      // console.log(response.data[0].client.image_url)

      // console.log("requests: ", requests)

    } catch (error) {
      console.error('Error fetching user data: ', error);
    }
  };

  // //fetch requests every 5 seconds
  useEffect(() => {
    //   // Fetch immediately for the first time
    fetchRequests();
    //   const interval = setInterval(() => {
    //     fetchRequests();
    //   }, 5000); // (fetch every 5 seconds)

    //   // Clear the interval when the component is unmounted
    //   return () => clearInterval(interval);
  }, []);

  //patch requests
  const handleaccept = async () => {

    const token = 'JWT ' + localStorage.getItem("token");
    const url = 'https://ghablameh.fiust.ir/api/v1/organizations/join-requests/' + IsSelectedUser.id + '/'

    try {
      const response = await axios.patch(url, { status: 'A' }, {
        headers: {
          'Authorization': token
        }
      });
      if (response.status === 200) {
        // console.log('formData submitted successfully');
        fetchRequests();
      } else {
        alert("مشکلی پیش آمده.در زمان دیگری مجددا امتحان کنید.")
        // console.log('formData submission failed:', errorData);
      }
    } catch (error) {
      // console.error('An error occurred:', error);
      alert("مشکلی پیش آمده.در زمان دیگری مجددا امتحان کنید.")
    }
    setShowAcceptModel(false);
  };

  const handlereject = async () => {

    const token = 'JWT ' + localStorage.getItem("token");
    const url = 'https://ghablameh.fiust.ir/api/v1/organizations/join-requests/' + IsSelectedUser.id + '/'

    try {
      const response = await axios.patch(url, { status: 'R' }, {
        headers: {
          'Authorization': token
        }
      });

      if (response.status === 200) {
        // console.log('formData submitted successfully');
        fetchRequests();
      } else {
        // console.log('formData submission failed:', response.data);
        alert("مشکلی پیش آمده.در زمان دیگری مجددا امتحان کنید.")
      }
    } catch (error) {
      // console.error('An error occurred:', error);
      alert("مشکلی پیش آمده.در زمان دیگری مجددا امتحان کنید.")
    }

    setShowRejectModel(false);
  };

  //modals
  const [showRejectModel, setShowRejectModel] = useState(false);
  const [showAcceptModel, setShowAcceptModel] = useState(false);

  const onClose = () => {
    setShowRejectModel(false);
    setShowAcceptModel(false);
  };

  const handleOnClose = (e) => {
    if (e.target.id === "close") onClose();
  };

  const handleRjectModalOpen = (user) => {
    // console.log(user)
    setShowRejectModel(true)
    setIsSelectedUser(user)
  }

  const handleAcceptModalOpen = (user) => {
    // console.log(user)
    setShowAcceptModel(true)
    setIsSelectedUser(user)
  }

  const ListOfUserRequests = () => (
    <div style={{ border: '1px solid rgb(38, 87, 124)', borderRadius: '8px', width: '50vw', backgroundColor: 'rgba(255, 255, 255,0.6)' }} className="w-64  font-medium text-gray-900  rounded-lg">
      <h2 className="text-xl font-semibold text-gray-800  text-center py-3 px-2" style={{ borderBottom: '1px solid rgb(38, 87, 124)', fontSize: isBigScreen ? '1.3rem' : '1rem' }}> لیست درخواست ها </h2>

      {isBigScreen && requests.length > 0 && (
        <ul className="w-full">
          {requests.map((user, index) => (
            <li key={index} style={{ borderBottom: index === requests.length - 1 ? 'none' : '1px solid rgb(38, 87, 124)' }} className="px-4 py-2">
              <div className="flex items-center p-2 flex-row justify-between mx-3 my-1">
                <div className="flex items-center">
                  <CheckIcon className="h-7 w-7 cursor-pointer ml-5" style={{ color: 'rgb(38, 87, 124)' }} onClick={() => handleAcceptModalOpen(user)} />
                  <XIcon className="h-7 w-7 cursor-pointer ml-2" style={{ color: 'rgb(38, 87, 124)' }} onClick={() => handleRjectModalOpen(user)} />
                </div>
                {/* <div style={{ backgroundColor: 'rgb(38, 87, 124)', color: 'white', borderRadius: '5px', height: '3vh', width: '5vw' }} className="rounded p-1 mx-5 flex  items-center justify-center">
                  {user.organization_name}
                </div> */}
                <div className="flex flex-rowitems-center">
                  <div className="ml-3 text-base">
                    <div style={{ fontSize: '1.3rem' }}>{user.client.first_name} {user.client.last_name}</div>
                  </div>
                  {/* {user.client.image_url ? (
                    // <img src={user.avatar} alt={`${user.client_name}`} className="h-11 w-11 rounded-full" />
                    <div className="overflow-hidden h-40" style={{ padding: '10px' }}>
                      <img src={'https://ghablameh.fiust.ir' + user.client.image_url} className="w-4 h-4 rounded-lg" style={{ objectFit: 'contain' }} />
                    </div>
                  ) : (
                    <Avatar name={`${user.client.first_name}`} size={44} round={true} maxInitials={1} />
                  )} */}


<div className={`flex items-center ml-1`}>
          {user.client.image_url ? (
            <img src={'https://ghablameh.fiust.ir' + user.client.image_url} alt={`${user.client_name}`} style={{ width: isBigScreen ? "2.7rem" : "2.1rem", height: isBigScreen ? "2.7rem" : "2.1rem", borderRadius: "50%" }} />
          ) : (
            <Avatar
              // onClick={toggleDropdown}
              name={`${user.client.first_name}`} 
              size={isBigScreen ? "50" : "45"}
              round={true}
              maxInitials={1}
            />
          )}
        </div>



                </div>


              </div>
            </li>
          )
          )}
        </ul>
      )}

      {!isBigScreen && requests.length > 0 && (
        <ul style={{ minWidth: '50vw' }}>
          {requests.map((user, index) => (
            <li key={index} style={{ borderBottom: index === requests.length - 1 ? 'none' : '1px solid rgb(38, 87, 124)' }} className="px-1 py-2">
              <div className="flex flex-row items-center ">
                {/* justify-between */}
                {/* <div className="flex items-center justify-end"> */}
                <CheckIcon className="cursor-pointer ml-1" style={{ color: 'rgb(38, 87, 124)', height: '20px', width: '20px' }} onClick={() => handleAcceptModalOpen(user)} />
                <XIcon className="cursor-pointer ml-5" style={{ color: 'rgb(38, 87, 124)', height: '20px', width: '20px' }} onClick={() => handleRjectModalOpen(user)} />
                {/* </div> */}

                {/* <div className="flex items-center flex-row justify-end"> */}
                <div className="felx items-center justify-center mr-auto text-xs m-1">
                  <div className="m-1" style={{ fontSize: '1rem' }}>{user.client_name}</div>
                </div>
                {/* <div>
                    <div style={{ fontSize: '1rem', marginRight: '2rem' }}> -{index + 1} </div>
                  </div> */}

                {/* </div> */}

              </div>
            </li>
          )
          )}
        </ul>
      )}

      {requests.length == 0 && (
        <ul className="w-full">
          <h3 className="text-xl font-light text-gray-800 pt-3 pb-2 pr-3 text-right p-6 m-2">درخواستی برای اضافه شدن به بوفه وجود ندارد</h3>
        </ul>
      )}

      {showRejectModel && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div id="close" onClick={handleOnClose} className={`${styles['modal-me']} bg-white rounded p-2`} style={{ position: 'absolute' }}>
            <div className="flex flex-col items-center">
              <div className="text-center mb-4 mx-2 mt-2">آیا از رد کردن این درخواست مطمئن هستید؟</div>
              <div className="flex justify-center space-x-3">
                <div className='mx-2'>
                  <button style={{ background: '#ff5e14' }} className="text-white font-bold py-1 px-3 rounded" onClick={() => setShowRejectModel(false)}>خیر</button>
                </div>
                <div className='mx-2'><button style={{ background: 'rgb(38, 87, 124)' }} className="text-white font-bold py-1 px-3 rounded" onClick={() => handlereject()}>بله</button>
                </div></div>
            </div>
          </div>
        </div>
      )}

      {showAcceptModel && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div id="close" onClick={handleOnClose} className={`${styles['modal-me']} bg-white rounded p-2`} style={{ position: 'absolute' }}>
            <div className="flex flex-col items-center">
              <div className="text-center mb-4 mx-2 mt-2">آیا از قبول کردن این درخواست مطمئن هستید؟</div>
              <div className="flex justify-center space-x-3">
                <div className='mx-2'>
                  <button style={{ background: '#ff5e14' }} className="text-white font-bold py-1 px-3 rounded" onClick={() => setShowAcceptModel(false)}>خیر</button>
                </div>
                <div className='mx-2'><button style={{ background: 'rgb(38, 87, 124)' }} className="text-white font-bold py-1 px-3 rounded" onClick={() => handleaccept()}>بله</button>
                </div></div>
            </div>
          </div>
        </div>
      )}
    </div>
  );

  return (
    <div>
      <div className={`${styles['main-content']} flex flex-col`}>
        <Navbarparent />
        <div className='flex flex-grow justify-center items-center m-3'>
          {ListOfUserRequests()}
        </div>
      </div>
    </div>
  );
}

export default ListOfJoinRequests;