import { useState, useEffect } from 'react';
import axios from 'axios';
import styles from './OrgPage.module.css';
import { Link } from 'react-router-dom';
import Navbarparent from '../components/navbarparent.jsx';

function OrgPage() {
  const [peopleData, setPeopleData] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [newBuffetName, setNewBuffetName] = useState('');
  const [editBuffet, setEditBuffet] = useState(null);
  const [editBuffetName, setEditBuffetName] = useState('');
  const [deleteConfirmation, setDeleteConfirmation] = useState(false);
  const [deleteBuffetId, setDeleteBuffetId] = useState(null);
  const [error, setError] = useState("");
  const [mode, setMode] = useState('');
  // Define your API endpoint and token
  const API_ENDPOINT = 'https://ghablameh.fiust.ir/api/v1/buffets/';
  const TOKEN = localStorage.getItem('token');

  // useEffect(() => {
  //   // Axios request configuration
  //   const axiosConfig = {
  //     headers: {
  //       'Authorization': `JWT ${TOKEN}`
  //       // Add other headers as needed
  //     }
  //   };

  //   // Fetch data using Axios when the component mounts
  //   axios.get(API_ENDPOINT, axiosConfig)
  //     .then(response => {
  //       // Update state with fetched data
  //       setPeopleData(response.data);
  //     })
  //     .catch(error => {
  //       console.error('Error fetching data:', error);
  //     });
  // }, []);

  const fetchData = () => {
    // Axios request configuration
    const axiosConfig = {
      headers: {
        'Authorization': `JWT ${TOKEN}`
        // Add other headers as needed
      }
    };
  
    // Fetch data using Axios
    axios.get(API_ENDPOINT, axiosConfig)
      .then(response => {
        // Update state with fetched data
        setPeopleData(response.data);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
      });
  };
  
  // Function to fetch data initially
  const fetchDataInitially = () => {
    fetchData();
  };
  
  // Call fetchDataInitially to fetch data when the component loads
  fetchDataInitially();
  
  // Interval for fetching data every 5 seconds
  // const intervalTime = 5 * 1000; // 5 seconds in milliseconds
  // setInterval(() => {
  //   fetchData();
  // }, intervalTime);

  const handleDeleteConfirmation = (id) => {
    setDeleteBuffetId(id);
    setDeleteConfirmation(true);
  };

  const handleDelete = () => {
    // Define your delete endpoint
    const deleteEndpoint = `${API_ENDPOINT}${deleteBuffetId}/`;

    // Axios request configuration for delete operation
    const axiosConfig = {
      headers: {
        'Authorization': `JWT ${TOKEN}`
        // Add other headers as needed
      }
    };

    // Send delete request using Axios
    axios.delete(deleteEndpoint, axiosConfig)
      .then(response => {
        // If delete is successful, update state to remove the deleted entity
        setPeopleData(prevData => prevData.filter(person => person.id !== deleteBuffetId));
        // Close the delete confirmation modal
        setDeleteConfirmation(false);
      })
      .catch(error => {
        console.error('Error deleting data:', error);
      });
  };

  const handleCreateBuffet = () => {
    setError('');
    //check if the name is empty
    if (!newBuffetName) {
      setError(' نام بوفه را وارد کنید. ');
      return;
    }
    // Define the data to be sent in the POST request
    const postData = {
      name: newBuffetName
    };

    // Axios request configuration for create operation
    const axiosConfig = {
      headers: {
        'Authorization': `JWT ${TOKEN}`,
        'Content-Type': 'application/json'
        // Add other headers as needed
      }
    };

    // Send POST request using Axios
    axios.post(API_ENDPOINT, postData, axiosConfig)
      .then(response => {
        // If creation is successful, update state with the new buffet
        setPeopleData(prevData => [...prevData, response.data]);
        // Reset the input field and close the modal
        setNewBuffetName('');
        setShowModal(false);
      })
      .catch(error => {
        console.error('Error creating buffet:', error);
      });
  };

  const handleEdit = () => {
    setError('');
    //check if the name is empty
    if (!editBuffetName) {
      setError(' نام بوفه را وارد کنید. ');
      return;
    }
    // Define the endpoint for editing the buffet
    const editEndpoint = `${API_ENDPOINT}${editBuffet.id}/`;

    // Define the data to be sent in the PUT request
    const putData = {
      name: editBuffetName
      // Add other fields as needed
    };

    // Axios request configuration for edit operation
    const axiosConfig = {
      headers: {
        'Authorization': `JWT ${TOKEN}`,
        'Content-Type': 'application/json'
        // Add other headers as needed
      }
    };

    // Send PUT request using Axios
    axios.put(editEndpoint, putData, axiosConfig)
      .then(response => {
        // If edit is successful, update state with the edited buffet
        setPeopleData(prevData => prevData.map(buffet => buffet.id === editBuffet.id ? response.data : buffet));
        // Reset the state and close the modal
        setEditBuffet(null);
        setEditBuffetName('');
        setShowModal(false);
      })
      .catch(error => {
        console.error('Error editing buffet:', error);
      });
  };
  const hanleclickjadval = (person) => {
    localStorage.setItem("Boofeh", person.name);
  };
  return (
    <div >
      <Navbarparent />
      {/* <div className=''>*/}
      <div className={styles.containment_boof} >
        <div className={styles.app}>
          <div className='flex flex-row justify-between items-center'>

            <div className={`${styles.itemscenter} flex-grow`}>
              {showModal && (
                <div className="fixed z-10 inset-0 overflow-y-auto">
                  <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
                    <div className="fixed inset-0 transition-opacity">
                      <div className="absolute inset-0 bg-black opacity-50"></div>
                    </div>
                    <span className="hidden sm:inline-block sm:align-middle sm:h-screen"></span>&#8203;
                    <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
                      <div className="bg-white p-6 flex flex-col items-center text-center">
                        <h3 className="text-lg mb-5 font-medium text-gray-900" id="modal-title">
                          {/* {editBuffet ? 'ویرایش بوفه' : 'ساخت بوفه جدید'} */}
                          {mode === 'edit' ? 'ویرایش بوفه' : 'ساخت بوفه جدید'}
                        </h3>

                        <div className="flex flex-col justify-center items-start my-2">
                          <input
                            type="text"
                            value={mode === 'edit' ? editBuffetName : newBuffetName}
                            onChange={(e) => {
                              mode === 'edit' ? setEditBuffetName(e.target.value) : setNewBuffetName(e.target.value);
                              setError('');
                            }}
                            className="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            placeholder="نام بوفه جدید"
                          />
                          {error && <span className='text-red-600 mt-3'>{error}</span>}
                        </div>

                      </div>
                      <div className="bg-gray-50 px-4 py-4 sm:px-6 sm:flex sm:flex-row-reverse justify-center">
                        <button onClick={mode === 'edit' ? handleEdit : handleCreateBuffet} type="button" className=" mr-2 w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-template-custom-orange text-base font-medium text-white hover:bg-orange-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-orange-500 sm:ml-3 sm:w-auto sm:text-sm">
                          {/* {editBuffet ? 'ویرایش' : 'ایجاد'} */}
                          {mode === 'edit' ? 'ویرایش ' : 'ایجاد  '}
                        </button>
                        <button onClick={() => { setShowModal(false); setError(''); }} type="button" className=" mr-2 mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 sm:mt-0 sm:w-auto sm:text-sm">
                          لغو
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              )}

              {/* Delete Confirmation Modal */}
              {deleteConfirmation && (
                <div className="fixed z-10 inset-0 flex items-center justify-center min-h-screen text-center overflow-y-auto">
                  <div className="absolute inset-0 bg-black opacity-50 transition-opacity"></div>
                  <div className="bg-white px-16 py-1 inline-block align-middle rounded-lg text-left shadow-xl transform transition-all my-8 max-w-sm sm:max-w-md md:max-w-lg lg:max-w-xl xl:max-w-2xl mx-5">
                    <div className="p-6 sm:p-6">
                      <div className="text-center sm:items-center">
                        <h3 className="text-lg leading-6 font-medium text-gray-900 mb-6" id="modal-title">آیا از حذف بوفه مطمئن هستید؟</h3>
                        <div className="flex items-center justify-center">
                          <button onClick={handleDelete} className="px-4 py-2 mx-2 bg-template-custom-orange text-white font-semibold rounded-md hover:bg-orange-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500">
                            بله
                          </button>
                          <button onClick={() => setDeleteConfirmation(false)} className="px-4 py-2 bg-gray-600 text-white font-semibold rounded-md hover:bg-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500">
                            لغو
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              )}


              <div className={styles.transform}>

                <button onClick={() => { setMode('create'); setShowModal(true); }} className="px-4 mt-36 w-52 py-3 font-medium text-white bg-template-custom-orange rounded-md hover:bg-orange-500 focus:outline-none focus:shadow-outline-red active:bg-red-500 transition duration-150 ease-in-ou text-sm">ساخت بوفه جدید</button>

                <div className='m-10 grid lg:grid-cols-5 md:grid-cols-5 mt-5 mb-5'>
                  <div className='lg:col-start-2 lg:col-span-3 md:col-start-2 md:col-span-3'>
                    <table className="table-auto min-w-full bg-white bg-opacity-60 rounded-lg divide-y divide-template-custom-orange">
                      <thead>
                        <tr>
                          <th className="px-6 py-5 text-base font-bold text-template-custom-orange uppercase tracking-wider">نام بوفه</th>
                          <th className="px-6 py-5 text-base font-bold text-template-custom-orange uppercase tracking-wider">عملیات</th>
                          {/* <th className="px-6 py-3 text-xs font-bold text-template-custom-orange uppercase tracking-wider">
                          </th> */}
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-template-custom-blue">
                        {peopleData.map(person => (
                          <tr key={person.id}>
                            {/* Column 1: Person Name */}
                            <td className="px-6 py-5 whitespace-nowrap text-base">{person.name}</td>

                            {/* Column 2: Operation Buttons */}
                            <td className="px-6 py-5 whitespace-nowrap">
                              {/* Edit Button */}
                              <button onClick={() => { setMode('edit'); setEditBuffet(person); setEditBuffetName(person.name); setShowModal(true); }} className="ml-2 px-3 py-2 font-medium text-white bg-sky-800 rounded-md hover:bg-sky-700 focus:outline-none focus:shadow-outline-blue active:bg-blue-600 transition duration-150 ease-in-out">ویرایش</button>

                              {/* Delete Button */}

                              <button onClick={() => handleDeleteConfirmation(person.id)} className="ml-2 px-3 py-2 font-medium text-white bg-template-custom-orange rounded-md hover:bg-orange-500 focus:outline-none focus:shadow-outline-red active:bg-red-600 transition duration-150 ease-in-out">حذف</button>
                              {/* Table Button */}
                              <Link to="/weeklymenu2">
                                <button onClick={() => { hanleclickjadval(person) }} className="ml-2 px-3 py-2 font-medium text-white bg-gray-600 rounded-md hover:bg-gray-500 focus:outline-none focus:shadow-outline-blue active:bg-gray-600 transition duration-150 ease-in-out">مشاهده جدول غذایی </button>
                              </Link>

                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        {/* </div> */}
      </div>
    </div>
  );
}

export default OrgPage;
