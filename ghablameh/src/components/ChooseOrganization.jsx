// import React from "react";
// import requests from "../APIs/AuthManager";
// import orgrequests from "../APIs/Organizations";
// import Swal from "sweetalert2";
// const ChooseOrganization = async () => {
//   let organizations = await requests.GetOrganizations();

//   const handleJoin = async () => {
//     let orgs = document.querySelectorAll(".organizationCheckbox");
//     let choosedOrgs = [];
//     for (let i = 0; i < orgs.length; i++) {
//       if (orgs[i].checked) {
//         choosedOrgs.push(orgs[i]);
//       }
//     }
//     for (let i = 0; i < choosedOrgs.length; i++) {
//       let success = await orgrequests.JoinOrganization(choosedOrgs[i].id);
//       if (!success) {
//         Swal.fire({
//           title: "خطا",
//           text: "ارسال در خواست با شکست مواجه شد!",
//           icon: "error"
//         });
//         return;
//       }
//       Swal.fire({
//         title: "درخواست موفق",
//         text: "درخواست عضویت شما با موفقیت ارسال شد!",
//         icon: "success"
//       });
//     }
//   };
//   return (
//     <>
//       <div className="content-center border border-sky-800 rounded py-4 my-20">
//         <div className="max-w-sm mx-auto">
//           <label
//             htmlFor="countries_multiple"
//             className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
//           >
//             <svg
//               className="w-6 h-6 text-gray-800 dark:text-white"
//               aria-hidden="true"
//               xmlns="http://www.w3.org/2000/svg"
//               width="24"
//               height="24"
//               fill="none"
//               viewBox="0 0 24 24"
//               style={{ display: "inline-block" }}
//             >
//               <path
//                 stroke="currentColor"
//                 strokeLinecap="round"
//                 strokeLinejoin="round"
//                 strokeWidth="2"
//                 d="M6 4h12M6 4v16M6 4H5m13 0v16m0-16h1m-1 16H6m12 0h1M6 20H5M9 7h1v1H9V7Zm5 0h1v1h-1V7Zm-5 4h1v1H9v-1Zm5 0h1v1h-1v-1Zm-3 4h2a1 1 0 0 1 1 1v4h-4v-4a1 1 0 0 1 1-1Z"
//               />
//             </svg>
//             سازمان مورد نظر خود را انتخاب نمایید
//           </label>
//           <div
//             className="border border-sky-800 rounded p-1"
//             style={{ maxHeight: "300px", overflowY: "scroll" }}
//           >
//             {organizations[0].id === 0 ? (
//               <p className="p-1 text-red-600">{organizations[0].name}</p>
//             ) : (
//               organizations.map((item) => (
//                 <div className="bg-gray-300 p-1 rounded my-2" key={item.id}>
//                   {item.name}
//                   <input
//                     key={item.id}
//                     type="checkbox"
//                     id={item.id}
//                     value="true"
//                     className="organizationCheckbox w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600 my-1"
//                     style={{ float: "left" }}
//                   />
//                 </div>
//               ))
//             )}
//           </div>
//           <div className="mt-2 mx-36">
//             <button
//               className="w-full bg-sky-800 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-full"
//               onClick={handleJoin}
//             >
//               تایید
//             </button>
//           </div>
//         </div>
//       </div>
//     </>
//   );
// };

// export default ChooseOrganization;
import React, { useState, useEffect } from "react";
import requests from "../APIs/AuthManager";
import orgrequests from "../APIs/Organizations";
import Swal from "sweetalert2";
import styles from '../styles/Swal.module.css';

const ChooseOrganization = () => {
  const [organizations, setOrganizations] = useState([]);
  const [isLoading, setIsLoading] = useState(false);


    const fetchOrganizations = async () => {
      setIsLoading(true);
      try {
        const data = await requests.GetOrganizations();
        setOrganizations(data);
        // console.log('asdfghjkl',data)
      } catch (error) {
        console.error("Error fetching organizations:", error);
      }
      setIsLoading(false);
    };

useEffect(() => {
  fetchOrganizations();
  // const intervalId = setInterval(() => {
  //   fetchOrganizations();
  // }, 5000); //5 seconds

  // return () => clearInterval(intervalId); // Clear the interval when the component is unmounted 

}, []); 

  const handleJoin = async () => {
    let orgs = document.querySelectorAll(".organizationCheckbox");
    let choosedOrgs = [];
    for (let i = 0; i < orgs.length; i++) {
      if (orgs[i].checked) {
        choosedOrgs.push(orgs[i]);
      }
    }
    for (let i = 0; i < choosedOrgs.length; i++) {
      try {
        const success = await orgrequests.JoinOrganization(choosedOrgs[i].id);
        if (!success) {
          Swal.fire({
            title: "خطا",
            text: "ارسال درخواست ناموفق بود!",
            icon: "error",
            confirmButtonText: 'تایید',
            customClass: {
              confirmButton: styles.confirmButton,
            }
          });
          return;
        }
        Swal.fire({
          title: "درخواست موفق",
          text: "درخواست عضویت شما با موفقیت ارسال شد!",
          icon: "success",
          confirmButtonText: 'تایید',
          customClass: {
            confirmButton: styles.confirmButton,
          }
        });
      } catch (error) {
        console.error("Error joining organization:", error);
        Swal.fire({
          title: "خطا",
          text: "ارسال درخواست ناموفق بود!",
          icon: "error",
          confirmButtonText: 'تایید',
          customClass: {
            confirmButton: styles.confirmButton,
          }
        });
      }
    }
  };

  return (
    <div className={`flex flex-col  items-center justify-center`}>
      <div className="content-center rounded py-4 my-20  ">
        <div className="max-w-sm mx-auto flex flex-col  items-center justify-center">

          <label
            htmlFor="countries_multiple"
            className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
          >
            <div className="flex flex-row items-center justify-center mb-5">
              <svg
                className="w-7 h-7 text-gray-800 dark:text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
                style={{ display: "inline-block" }}
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M6 4h12M6 4v16M6 4H5m13 0v16m0-16h1m-1 16H6m12 0h1M6 20H5M9 7h1v1H9V7Zm5 0h1v1h-1V7Zm-5 4h1v1H9v-1Zm5 0h1v1h-1v-1Zm-3 4h2a1 1 0 0 1 1 1v4h-4v-4a1 1 0 0 1 1-1Z"
                />
              </svg>

              <div className="mx-3 text-lg">سازمان مورد نظر خود را انتخاب نمایید.</div>

            </div>

          </label>

          <div
            className=" mb-5 rounded-xl p-4 bg-sky-900 w-80"
            // style={{ maxHeight: "350px", overflowY: "scroll", background: '' }}
          >
            {/* border border-sky-800 */}

            {organizations.length === 0 ?
              isLoading ? (
                <div className="flex justify-center items-center">
                <div className={styles.spinner}></div>
                </div>
              ) : (
                <p className="p-1 text-white"> سازمانی یافت نشد. </p>
              ) : (
                <div className="bg-white bg-opacity-85 rounded-lg p-0.5">
                
                  {organizations.map((item, index) => (
                    <div className="p-2 m-2" key={item.id}  style={{ borderBottom: index === requests.length - 1 ? 'none' : '1px solid rgb(38, 87, 124)' }}>
                      {item.name}
                      <input
                        type="checkbox"
                        id={item.id}
                        value="true"
                        className="organizationCheckbox w-4 h-4 text-blue-600  rounded focus:ring-sky-800 dark:focus:ring-sky-900 dark:ring-offset-gray-800 focus:ring-2 bg-gray-100 border-gray-300 dark:bg-gray-700 dark:border-gray-600 my-1"
                        style={{ float: "left" }}
                      />
                    </div>
                  ))}
                </div>
              )}

          </div>
          <div className="mt-2 mx-36">
            <button
              className="w-full bg-sky-800 hover:bg-orange-500 text-white  py-2 px-4 rounded-full"
              onClick={handleJoin}
            >
              تایید
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ChooseOrganization;