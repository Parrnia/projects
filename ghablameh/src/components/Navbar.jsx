/* eslint-disable no-unused-vars */
import { useState, useEffect, useRef } from "react";
import Avatar from "react-avatar";
import styles from "../styles/Navbar.module.css";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import AuthManager from "../APIs/AuthManager";
import useMediaQuery from "@mui/material/useMediaQuery";
import UserWallet from "./wallet";
import PropTypes from "prop-types";
import { ArrowCircleRightIcon } from '@heroicons/react/solid'
import {
  HiArrowSmRight,
  HiChartPie,
  HiInbox,
  HiShoppingBag,
  HiTable,
  HiUser,
  HiViewBoards,
} from "react-icons/hi";
import DefaultSidebar from "./Sidebar";
import Notificationbox from "./Notificationbox";
import bgimage from '/wave3.png'

// function Navbar() {
const Navbar = ({ openWallet, setOpenWallet }) => {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef(null);
  const [userData, setUserData] = useState(null);
  const [adminData, setAdminData] = useState(null);
  const isBigScreen = useMediaQuery("(min-width: 600px)");
  const navigate = useNavigate();
  const sideBar = useRef(null);
  const notification = useRef(null);
  const [userType, setUserType] = useState(null);

  //fetch user data
  useEffect(() => {
    const decideAndFetchData = async () => {
      try {
        if (AuthManager.isLoggedIn()) {
          const token = AuthManager.getToken();
          const userTypeResult = await AuthManager.orguser();
          setUserType(userTypeResult);

          if (userTypeResult === 1) {
            const orgResponse = await axios.get(
              "https://ghablameh.fiust.ir/api/v1/organizations/me/",
              { headers: { Authorization: "JWT " + token } }
            );

            setAdminData(orgResponse.data);
          } else if (userTypeResult === 2) {
            const userResponse = await axios.get(
              "https://ghablameh.fiust.ir/api/v1/clients/me/",
              { headers: { Authorization: "JWT " + token } }
            );
            setUserData(userResponse.data);
          }
        }
      } catch (error) {
        console.error("Error: ", error);
      }
    };

    decideAndFetchData();
  }, []);

  //dropdown
  useEffect(() => {
    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, []);

  const toggleDropdown = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleClickOutside = (e) => {
    if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
      setIsDropdownOpen(false);
    }
  };

  const handleOpenSidebar = () => {
    // console.log(sideBar.current);
    let displayStatus = sideBar.current.style.display;
    if (displayStatus !== "block") sideBar.current.style.display = "block";
    else sideBar.current.style.display = "none";
  };

  const hanldeOpenNotifications = () => {
    let displayStatus = notification.current.style.display;
    if (displayStatus !== "block") notification.current.style.display = "block";
    else notification.current.style.display = "none";
  };

  //log out
  async function handleLogout() {
    setIsDropdownOpen(false); // Close the dropdown menu on logout

    setUserData(null); // Clear user data
    localStorage.removeItem("token");
    localStorage.removeItem("refresh-token");
    // console.log(AuthManager.isLoggedIn());
    navigate("/landing");
    window.location.reload();
  }

  function UserAvatar() {
    if (AuthManager.isLoggedIn()) {
      let displayName;
      let image_src;

      if (userType === 2 && userData) {
        displayName = userData.first_name;
        image_src = userData.image_url ? "https://ghablameh.fiust.ir" + userData.image_url : null;
      } else if (userType === 1 && adminData) {
        displayName = adminData.admin_first_name;
        image_src = adminData.image_url ? "https://ghablameh.fiust.ir" + adminData.image_url : null;
      }

      return (
        <div className={`flex items-center ml-1`}>
          {image_src ? (
            <img src={image_src} alt="Profile" style={{ width: isBigScreen ? "2.7rem" : "2.1rem", height: isBigScreen ? "2.7rem" : "2.1rem", borderRadius: "50%" }} />
          ) : (
            <Avatar
              // onClick={toggleDropdown}
              name={displayName}
              size={isBigScreen ? "50" : "45"}
              round={true}
              maxInitials={1}
            />
          )}
        </div>
      );
    }
  }

  function Username() {
    if (AuthManager.isLoggedIn()) {
      let displayName;
      if (userType === 2 && userData) {
        if (isBigScreen) {
          displayName = userData.first_name + ' ' + userData.last_name;
        } else {
          displayName = userData.first_name;
        }

      } else if (userType === 1 && adminData) {
        if (isBigScreen) {
          displayName = adminData.admin_first_name + ' ' + adminData.admin_last_name;
        } else {
          displayName = adminData.admin_first_name;
        }
      }

      if (displayName) {
        return (
          <h6
            className={`${styles.vazir} text-white`}
            style={{ marginLeft: '10px', fontSize: isBigScreen ? "20px" : "16px" }}
          >
            {displayName}
          </h6>

        );
      }
    }
  }

  function LogInButton() {
    if (!AuthManager.isLoggedIn()) {
      return (
        <div className={`flex justify-between items-center my-4`}>
          <div style={{}}>
            <Link
              to="/login"
              className={`text-white`}
              style={{ fontSize: "1rem", fontFamily: "vazir" }}
            >
              وارد شوید
            </Link>
          </div>
          <div>
            <h6 className={`text-white pl-2`} style={{ fontSize: "23px", fontFamily: "vazir", marginRight: "10px", marginLeft: '7px' }}>/</h6>
          </div>
          <div>
            <Link
              to="/signup"
              className={`text-white`}
              style={{ fontSize: "1rem", fontFamily: "vazir" }}
            >
              ثبت نام کنید
            </Link>
          </div>
        </div>
      );
    }
  }

  return (
    <>
      <nav style={{ backgroundColor: "rgb(38, 87, 124)" }} className={`flex justify-between items-center w-full pl-5`} >


        {/* Elements - Logo */}
        <div className={`flex items-center justify-center`}>
          <div className={`flex items-center justify-center`}>

            {/* ghaablame icon */}
            <button
              className="flex items-center text-white ml-5 mr-6"
              type="button"
              data-twe-offcanvas-toggle
              data-twe-target="#offcanvasRight"
              aria-controls="offcanvasRight"
              data-twe-ripple-init
              data-twe-ripple-color="light"
              style={{ fontSize: "35px", fontFamily: "vazir" }}
            >
              {/* <Link to="/"> */}
              <img src={'./logo-orange.jpg'} onClick={handleOpenSidebar} alt={' قابلمه '} style={{ height: '1.7rem' }} />
              {/* </Link> */}
            </button>

            {/* wallet icon */}
            {AuthManager.isLoggedIn() && (
              <>
                <svg
                  onClick={() => setOpenWallet((prevState) => !prevState)}
                  style={{ height: "2rem", width: "2rem" }}
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="white"
                  className="w-6 h-6 flex"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M21 12a2.25 2.25 0 0 0-2.25-2.25H15a3 3 0 1 1-6 0H5.25A2.25 2.25 0 0 0 3 12m18 0v6a2.25 2.25 0 0 1-2.25 2.25H5.25A2.25 2.25 0 0 1 3 18v-6m18 0V9M3 12V9m18 0a2.25 2.25 0 0 0-2.25-2.25H5.25A2.25 2.25 0 0 0 3 9m18 0V6a2.25 2.25 0 0 0-2.25-2.25H5.25A2.25 2.25 0 0 0 3 6v3"
                  />
                </svg>
                {/* Notifications */}
                <svg
                  className="w-6 h-6 text-gray-100 dark:text-white m-5"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="none"
                  viewBox="0 0 22 22"
                  onClick={hanldeOpenNotifications}
                >
                  <path
                    stroke="currentColor"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth="2"
                    d="M12 5.365V3m0 2.365a5.338 5.338 0 0 1 5.133 5.368v1.8c0 2.386 1.867 2.982 1.867 4.175 0 .593 0 1.292-.538 1.292H5.538C5 18 5 17.301 5 16.708c0-1.193 1.867-1.789 1.867-4.175v-1.8A5.338 5.338 0 0 1 12 5.365ZM8.733 18c.094.852.306 1.54.944 2.112a3.48 3.48 0 0 0 4.646 0c.638-.572 1.236-1.26 1.33-2.112h-6.92Z"
                  />
                </svg>
              </>
            )}

          </div>
          {isBigScreen && (
            <div
              className={`flex items-center justify-end space-x-3`}
              style={{}}
            >
              <Link
                to="/"
                className={`text-white`}
                style={{ fontSize: "1.1rem", marginLeft: "0.5vw" }}
              >
                صفحه اصلی
              </Link>
            </div>
          )}
        </div>

        {/* Avatar, username and Login button */}
        <div ref={dropdownRef}>
          <div className={`flex justify-between items-center`}>
            {/* logout icon */}
            {(userData || adminData) && (
              <>
                <svg xmlns="http://www.w3.org/2000/svg" onClick={handleLogout} viewBox="0 0 24 24" fill="currentColor" className="size-6 m-2" style={{ color: 'white', width: isBigScreen ? "24px" : "22px", height: isBigScreen ? "24px" : "20px" }}>
                  <path fillRule="evenodd" d="M7.5 3.75A1.5 1.5 0 0 0 6 5.25v13.5a1.5 1.5 0 0 0 1.5 1.5h6a1.5 1.5 0 0 0 1.5-1.5V15a.75.75 0 0 1 1.5 0v3.75a3 3 0 0 1-3 3h-6a3 3 0 0 1-3-3V5.25a3 3 0 0 1 3-3h6a3 3 0 0 1 3 3V9A.75.75 0 0 1 15 9V5.25a1.5 1.5 0 0 0-1.5-1.5h-6Zm10.72 4.72a.75.75 0 0 1 1.06 0l3 3a.75.75 0 0 1 0 1.06l-3 3a.75.75 0 1 1-1.06-1.06l1.72-1.72H9a.75.75 0 0 1 0-1.5h10.94l-1.72-1.72a.75.75 0 0 1 0-1.06Z" clipRule="evenodd" />
                </svg>
                {Username()}
                {UserAvatar()}

              </>
            )}
            {LogInButton()}

          </div>

          {/* Dropdown */}
          {/* <div className={`absolute z-10 ${isDropdownOpen ? "" : "hidden" } rounded-lg shadow p-1`} style={{ backgroundColor: "rgb(38, 87, 124)", margin: "0.3vw" }}>
              <ul className={`py-1 text-sm text-white`}>
                <li>
                  <a className={`block px-4 py-2 hover:bg-gray-800 dark:hover:bg-gray-600`}>
                    userpanel
                  </a>
                </li>
                <li>
                  <a onClick={handleLogout} className={`block px-4 py-2 hover:bg-gray-800 dark:hover:bg-gray-600`}>
                    Logout
                  </a>
                </li>
              </ul>
            </div> */}

        </div>

      </nav>

      {/* sidebar */}
      <div
        style={{
          display: "none",
          position: "absolute",
          top: "5",
          // right: "0",
          maxHeight: "300px",
          // opacity:'100%'
        }}
        ref={sideBar}
      >
        <DefaultSidebar />
      </div>

      {/* notifications */}
      <div
        className={styles.notifbox}
        ref={notification}
      >
        {AuthManager.isLoggedIn() && <Notificationbox />}
        {/* <Notificationbox/> */}
      </div>

      {/* wallet */}
      {openWallet && <UserWallet open={openWallet} setOpen={setOpenWallet} />}
      <img src={bgimage} alt="background" className="mb-4" />
    </>
  );
};

// Navbar.propTypes = {
//   open: PropTypes.bool.isRequired,
//   setOpen: PropTypes.func.isRequired,
// };

export default Navbar;
