import React, { useEffect, useRef, useState } from "react";
import notifmanager from "../APIs/Notifications";
import io from "socket.io-client";
import AuthManager from "../APIs/AuthManager";
import jalaliMoment from 'jalali-moment';
import moment from 'moment';
import axios from "axios";
const Notificationbox = () => {
  const [data, setData] = useState([]);
  const [count, setCount] = useState(0);
  const Reads = useRef(null);
  const All = useRef(null);
  const content = useRef(null);

  useEffect(() => {
    const fetchData = async () => {
      const alldata = await notifmanager.GetAll();
      setData(alldata.data);
      setCount(alldata.count);
      const token = AuthManager.getToken();
      const ws = new WebSocket(`ws://ghablameh.fiust.ir/ws/notifications/?token=${token}`);
      ws.onopen = () => {
        console.log("SUUUUUUUUUUUUUUUUUUUUU")
      };
      ws.onmessage = (event)=> {
        const newMessage = event.data;
        setData((prevdata) => [...prevdata, newMessage]);
      }
      ws.onerror = (error) => {
        console.log('WebSocket error:', error);
      };
    };
    fetchData();
  }, []);

  const hanldeReads = () => {
    const readData = data.filter((m) => m.read === true);
    setData(readData);
    Reads.current.classList = "p-1 bg-sky-800 text-stone-100 text-center w-1/2";
    All.current.classList = "p-1 bg-sky-600 text-stone-100 text-center w-1/2";
  };

  const handleAll = async () => {
    const alldata = await notifmanager.GetAll();
    setData(alldata.data);
    setCount(alldata.count);
    Reads.current.classList = "p-1 bg-sky-600 text-stone-100 text-center w-1/2";
    All.current.classList = "p-1 bg-sky-800 text-stone-100 text-center w-1/2";
  };

  function gregorianToPersian(dateString) {
    let options = { year: 'numeric', month: 'long', day: 'numeric' };
    let today = new Date(dateString).toLocaleDateString('fa-IR', options);
    return today;
  }

  const MarkAsRead = async (event) => {
    const id = event.target.id;
    let token = AuthManager.getToken();
    await axios.put("https://ghablameh.fiust.ir/api/v1/notifications/" + id + "/", { read: 'true' }, { headers: { Authorization: "JWT " + token } });
    window.location.reload();
  }

  return (
    // <div className="border border-sky-800 rounded" style={{ width: "100%" }}>
    //   <div className="grid grid-cols-2">
    //     <div
    //       ref={Reads}
    //       style={{
    //         borderLeft: "1px black solid",
    //         borderBottom: "1px solid black",
    //         cursor: "pointer",
    //       }}
    //       className="p-1 bg-sky-800 text-stone-100 test-center"
    //       onClick={hanldeReads}
    //     >
    //       خوانده شده ها
    //     </div>
    //     <div
    //       ref={All}
    //       className="p-1 bg-cyan-700 text-stone-100 test-center"
    //       style={{ borderBottom: "1px solid black", cursor: "pointer" }}
    //       onClick={handleAll}
    //     >
    //       همه
    //       <span
    //         className="bg-red-400 text-red-900 text-xs font-medium me-2 px-1 py-0.5 rounded dark:bg-gray-700 dark:text-red-400 border border-red-700 mx-2"
    //       >
    //         {count}
    //       </span>
    //     </div>
    //   </div>
    //   <div className="p-1 m-1" ref={content} style={{maxHeight:"250px",overflowY:"scroll"}}>
    //     {data.length === 0 ? (
    //       <div className="text-stone-800 text-center">در حال حاضر اعلانی ندارید </div>
    //     ) : (
    //       data.map((m) => (
    //         <div
    //           key={m.id}
    //           className={m.read == "true" ? "bg-slate-300 p-2 mx-1 my-2 rounded" : "bg-slate-300 p-2 mx-1 my-2 rounded"}
    //         >
    //           <span>{m.title}</span>
    //           <div className="grid grid-cols-2" style={{ fontSize: "12px" }} id={m.id} onClick={MarkAsRead}>
    //             <div>{gregorianToPersian(m.created_at.slice(0,10))}</div>
    //           </div>
    //         </div>
    //       ))
    //     )}
    //   </div>
    // </div>
    <div className="border border-grey-400 rounded" style={{ width: "100%" }}>


      <div className="flex flex-row">

        {/* read */}
        <div
          ref={Reads}
          style={{
            // borderLeft: "1px black solid",
            // borderBottom: "1px solid black",
            cursor: "pointer",
          }}
          className="p-1 bg-sky-800 text-stone-100 text-center w-1/2"
          onClick={hanldeReads}
        >
          خوانده شده ها
        </div>

        {/* all */}
        <div
          ref={All}
          className="p-1 bg-sky-600 text-stone-100 text-center w-1/2"
          style={{ cursor: "pointer", display: "flex", justifyContent: "center", alignItems: "center" }}
          onClick={handleAll}
        >
          <span>همه</span>
          <span
            className="mx-2 bg-orange-400 text-dark text-xs font-medium px-1 py-0.5 rounded dark:bg-gray-70 dark:text-orange-500 text-center ml-2"
          >
            {count}
          </span>
        </div>

      </div>

      <div className="p-1 m-1" ref={content} style={{ maxHeight: "250px", overflowY: "scroll" }}>
        {data.length === 0 ? (
          <div className="text-stone-800 text-center p-2">در حال حاضر اعلانی ندارید </div>
        ) : (
          data.map((m) => (
            <div
              key={m.id}
              className="bg-slate-300 p-2 mx-1 my-2 rounded"
              style={{ backgroundColor: '#edf3fc' }}
            >
              <span className="text-base w-full text-right block">{m.title}</span>
          
              <div className="grid grid-cols-2" style={{ fontSize: "12px" }} id={m.id}
                onClick={MarkAsRead}
              >

                <div>{gregorianToPersian(m.created_at.slice(0, 10))}</div>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default Notificationbox;