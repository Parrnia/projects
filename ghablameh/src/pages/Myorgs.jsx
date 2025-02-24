
// export default Myorgs;
import React, { useRef, useState, useEffect } from "react";
import Navbar from "../components/Navbar";
import styles from "../styles/updateinfo.module.css";
import  "../styles/myorgs.css";
import organizationsRequest from '../APIs/Organizations';
import Navbarparent from "../components/navbarparent";
import { Link } from "react-router-dom";
const Myorgs = () => {
  const [orgs, setOrgs] = useState([]);
  const [filteredOrgs, setFilteredOrgs] = useState([]);
  const searchData = useRef(null);

  useEffect(() => {
    const fetchOrganizations = async () => {
      try {
        const gottenOrgs = await organizationsRequest.GetMyOrganizations();
        gottenOrgs.sort((a, b) => {
                  const order = ['P', 'R', 'A'];
                  return order.indexOf(a.status) - order.indexOf(b.status);
                });
        setOrgs(gottenOrgs);
        setFilteredOrgs(gottenOrgs);
        // console.log(gottenOrgs)
      } catch (error) {
        console.error('Error fetching organizations:', error);
      }
    };
    fetchOrganizations();
  }, []);
  function gregorianToPersian(dateString) {
    let options = { year: 'numeric', month: 'long', day: 'numeric' };
    let today = new Date(dateString).toLocaleDateString('fa-IR', options);
    // console.log(today);
    return today;
  }
  const handleSearch = () => {
    const filtered = orgs.filter((item) =>
      item.organization_name.toLowerCase().includes(searchData.current.value.toLowerCase())
    );
    setFilteredOrgs(filtered);
    // console.log(filtered)
  };

  return (
    <div>
      <Navbarparent />
      <div className={` mx-2 my-5 p-2`}>
        <div className=" flex items-center justify-center flex-col">
          <div className="flex items-center justify-center searchbarForMyOrgs"> 
          {/* grid grid-cols-12       className="col-span-10"        className="col-span-2 m-2" */}
            <div style={{display:"inline-block", width:"68%" , marginLeft:"1%"}}>
              <input
                ref={searchData}
                type="text"
                className={`${[styles.input]} rounded-lg`}
                placeholder="جست و جو ..."
              />
            </div>

            <div style={{display:"inline-block", width:"28%"}}>
              <button
                onClick={handleSearch}
                className="w-full bg-sky-800 hover:bg-sky-900 text-white px-2 rounded-lg"
                style={{ paddingTop: '0.62rem', paddingBottom: '0.62rem' }}
              >
                جست و جو
              </button>
            </div>

          </div>

        </div>

        <div className="mx-3 my-6">
          {filteredOrgs.length === 0 ? (
            <div className="flex items-center justify-center flex-col">
            <p className="text-sky-800 text-center my-5" style={{ fontSize: "24px" }}>
              سازمانی یافت نشد!
            </p>
            <Link to='/chooseOrg' className="p-2 text-white bg-orange-500 rounded-lg"> درخواست عضویت </Link>
            </div>
          ) : (
            filteredOrgs.map((item) => (
              <div
                key={item.id}
                className="border border-sky-800 rounded-lg p-2 pt-3 pr-3 my-2 m-2"
              >

                <div>
                  <div>
                    <p >نام سازمان :
                      <span className=" font-bold">{` ${item.organization_name}`}</span>
                    </p>
                  </div>
                  <div className="text-center my-5">
                    <span>
                      وضعیت :
                      <span
                        className={`text-green-800 text-sm font-medium me-2 px-2.5 py-0.5 rounded dark:text-green-300 m-3 ${item.status === 'P'
                          ? 'bg-yellow-100'
                          : item.status === 'A'
                            ? 'bg-green-200'
                            : 'bg-red-400'
                          }`}
                      >
                        {item.status === 'P'
                          ? 'درحال بررسی'
                          : item.status === 'A'
                            ? 'تایید شده'
                            : 'رد شده'}
                      </span>
                    </span>
                  </div>

                  <div>
                    <p style={{ direction: "ltr" }}>
                      تاریخ درخواست : {gregorianToPersian(item.created_at.slice(0, 10))}
                    </p>
                  </div>

                </div>

              </div>

            ))
          )}
        </div>

      </div>
    </div>
  );
};

export default Myorgs;