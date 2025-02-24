// import React from "react";
// import Navbar from "../components/Navbar";
// import Footer from "../components/footer";
// import WeeklyMenuTable from "../components/WeeklyMenuTable";
// const WeeklyMenuPage = async () => {
//   return (
//     <>
//       <Navbar />
//       <div className=""></div>
//       <div style={{ width: "100%" }} className="px-5 py-3">
//         <div className="grid grid-cols-3 my-4 text-center">
//           <button className="scale-90 bg-template-custom-blue hover:bg-template-custom-orange text-white font-medium py-2 rounded-2xl ml-20 border-none cursor-pointer mt-1 mb-1">
//             <svg
//               className="w-6 h-6 text-white dark:text-white"
//               aria-hidden="true"
//               xmlns="http://www.w3.org/2000/svg"
//               width="24"
//               height="24"
//               fill="none"
//               viewBox="0 0 24 24"
//               style={{ display: "inline" }}
//             >
//               <path
//                 stroke="currentColor"
//                 strokeLinecap="round"
//                 strokeLinejoin="round"
//                 strokeWidth="2"
//                 d="m9 5 7 7-7 7"
//               />
//             </svg>
//             قبلی
//           </button>
//           <div className="border border-sky-800 rounded-lg px-5 py-3">
//             <svg
//               className="w-6 h-6 text-gray-800 dark:text-white"
//               aria-hidden="true"
//               xmlns="http://www.w3.org/2000/svg"
//               width="24"
//               height="24"
//               fill="none"
//               viewBox="0 0 24 24"
//               style={{ display: "inline" }}
//             >
//               <path
//                 stroke="currentColor"
//                 strokeLinecap="round"
//                 strokeLinejoin="round"
//                 strokeWidth="2"
//                 d="M4 10h16m-8-3V4M7 7V4m10 3V4M5 20h14a1 1 0 0 0 1-1V7a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1Zm3-7h.01v.01H8V13Zm4 0h.01v.01H12V13Zm4 0h.01v.01H16V13Zm-8 4h.01v.01H8V17Zm4 0h.01v.01H12V17Zm4 0h.01v.01H16V17Z"
//               />
//             </svg>
//             هفته جاری
//           </div>
//           <button className="scale-90 bg-template-custom-blue hover:bg-template-custom-orange text-white font-medium py-2 rounded-2xl mr-20 border-none cursor-pointer mt-1 mb-1" style={{ direction: "ltr" }}>

//             <svg
//               className="w-6 h-6 text-white dark:text-white"
//               aria-hidden="true"
//               xmlns="http://www.w3.org/2000/svg"
//               width="24"
//               height="24"
//               fill="none"
//               viewBox="0 0 24 24"
//               style={{ display: "inline" }}
//             >
//               <path
//                 stroke="currentColor"
//                 strokeLinecap="round"
//                 strokeLinejoin="round"
//                 strokeWidth="2"
//                 d="m15 19-7-7 7-7"
//               />
//             </svg>
//             بعدی
//           </button>
//         </div>
//         <WeeklyMenuTable />
//       </div>
//       <Footer />
//     </>
//   );
// };

// export default WeeklyMenuPage;
import { useRef } from "react";
import axios from "axios";
import requests from '../APIs/AuthManager';
import { useState, useEffect } from "react";
import Organizations from "../APIs/Organizations";
import Navbarparent from "../components/navbarparent";
const WeeklyMenuPage = () => {
  const date = new Date();
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  let month = date.getMonth() + 1;
  //alert(month)
  //alert(date)
  let year = date.getFullYear();
  //alert(year)
  const foodrow = useRef(null);
  const foodrow2 = useRef(null);
  const currentBuffet = useRef(null);
  const next = () => {
    date.setMonth(date.getMonth() + 1);
    month = date.getMonth();
    year = date.getFullYear();
    //alert(month)
  }
  useEffect(() => {
    const fetchData = async () => {
      try {
        let result = await Organizations.GetOrganizationBuffets();
        setData(result);
      } catch (error) {
        console.error("Error fetching data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);
  const handleBuffetChange = async () => {
    let buffetId = currentBuffet.current.value;
    //alert(buffetId);
    let data = [];
    const token = requests.getToken();
    data = await axios.get("https://ghablameh.fiust.ir/api/v1/buffets/" + buffetId + "/menus/", { headers: { Authorization: `JWT ${token}` } });
    if (data.data.length !== 0) {
      let listPK = data.data[0].id;
      let foods = await axios.get("https://ghablameh.fiust.ir/api/v1/buffets/" + buffetId + "/menus/" + listPK + "/meals/", { headers: { Authorization: `JWT ${token}` } });
      if (foods.data) {
        foodrow.current.innerText = foods.data[0].name;
        foodrow2.current.innerText = foods.data[1].name;
      }
    }
  }
  return (
    <>
      <Navbarparent/>
      <div className=""></div>
      <div style={{ width: "100%" }} className="px-5 py-3">
        <div className="grid grid-cols-3 my-4 text-center">
          <button className="scale-90 bg-template-custom-blue hover:bg-template-custom-orange text-white font-medium py-2 rounded-2xl ml-20 border-none cursor-pointer mt-1 mb-1">
            <svg
              className="w-6 h-6 text-white dark:text-white"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              fill="none"
              viewBox="0 0 24 24"
              style={{ display: "inline" }}
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="m9 5 7 7-7 7"
              />
            </svg>
            قبلی
          </button>
          <button
            className="scale-90 bg-template-custom-blue hover:bg-template-custom-orange text-white font-medium py-2 rounded-2xl border-none cursor-pointer mt-1 mb-1"
            style={{ direction: "ltr" }}
          >
            <div className="rounded-lgpy-3">

              <svg
                className="w-6 h-6 text-white dark:text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
                style={{ display: "inline" }}
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M4 10h16m-8-3V4M7 7V4m10 3V4M5 20h14a1 1 0 0 0 1-1V7a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1Zm3-7h.01v.01H8V13Zm4 0h.01v.01H12V13Zm4 0h.01v.01H16V13Zm-8 4h.01v.01H8V17Zm4 0h.01v.01H12V17Zm4 0h.01v.01H16V17Z"
                />
              </svg>
              ماه فعلی
            </div>
          </button>
          <button onClick={next}
            className="scale-90 bg-template-custom-blue hover:bg-template-custom-orange text-white font-medium py-2 rounded-2xl mr-20 border-none cursor-pointer mt-1 mb-1"
            style={{ direction: "ltr" }}
          >
            <svg
              className="w-6 h-6 text-white dark:text-white"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              fill="none"
              viewBox="0 0 24 24"
              style={{ display: "inline" }}
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="m15 19-7-7 7-7"
              />
            </svg>
            بعدی
          </button>
        </div>
        <div className="grid grid-cols-3 w-full">
          <div></div>
          <div className="
content-center w-full">
            <select className="rounded w-full" onChange={handleBuffetChange} ref={currentBuffet}>
              {loading ? (
                <option>Loading...</option>
              ) : (
                data.map((item) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))
              )}
            </select>
          </div>
          <div></div>
        </div>
        <p className="mt-5 mb-0">تاریخ ماه : {year}/{month}</p>
        <div className=''>
          <table className="border-collapse border w-full  border-blue-500 m-10 mx-auto">
            <thead>
              <tr className="bg-blue-500 text-white ">
                <th className="py-2 px-4 text-right">روز</th>
                <th className="py-2 px-4 text-right">صبحانه</th>
                <th className="py-2 px-4 text-right">نهار</th>
                <th className="py-2 px-4 text-right">شام</th>
              </tr>
            </thead>
            <tbody>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">شنبه</td>
                <td className="py-2 px-4" ref={foodrow}>25</td>
                <td className="py-2 px-4"></td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">یکشنبه</td>
                <td className="py-2 px-4" ref={foodrow2}>30</td>
                <td className="py-2 px-4">Los Angeles</td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">دوشنبه</td>
                <td className="py-2 px-4"></td>
                <td className="py-2 px-4"></td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">سه شنبه</td>
                <td className="py-2 px-4"></td>
                <td className="py-2 px-4"></td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">چهار شنبه</td>
                <td className="py-2 px-4"></td>
                <td className="py-2 px-4"></td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">پنج شنبه</td>
                <td className="py-2 px-4"></td>
                <td className="py-2 px-4"></td>
              </tr>
              <tr className="bg-white border-b border-blue-500">
                <td className="py-2 px-4">جمعه</td>
                <td className="py-2 px-4"></td>
                <td className="py-2 px-4"></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default WeeklyMenuPage;
