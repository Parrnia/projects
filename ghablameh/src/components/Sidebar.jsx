import { Sidebar, Menu, MenuItem, SubMenu } from "react-pro-sidebar";
import { Link } from "react-router-dom";
import AuthManager from '../APIs/AuthManager';
import { useState, useEffect } from 'react';

export default function DefaultSidebar() {
  const [flag, setFlag] = useState(null);

  useEffect(() => {
    const checkUserType = async () => {
      const userType = await AuthManager.orguser();
      setFlag(userType);
    };

    checkUserType();
  }, []);
  if (flag === null) {
    return (
      <div className='w-full flex items-center justify-center'>
        {/* این نشون داده نشه بهتره */}
        {/* <h3 className="text-xl font-light pt-10 pb-7 pr-3 text-right p-6 m-2 bg-sky-900 text-white "> منتظر بمانید... </h3> */}
      </div>
    );
  }

  const userSidebar = () => {
    return (
      <Sidebar style={{ boxShadow: "0px 3px 6px #00000029" }}  >
        <Menu
          menuItemStyles={{
            button: {
              ":hover": {
                color: "black",
                backgroundColor: "#EBE4D1"
              },
              color: "white",
            },
            root: {
              backgroundColor: "#26577C",
              // opacity: "5",
              boxShadow: "0px 3px 8px #00000029", // This line adds the shadow
              // marginRight:'2px'
            },
          }}
        >
          <MenuItem component={<Link to="/update" />}>
            <span style={{ marginRight: '10px' }}>
              تغییر اطلاعات کاربری
            </span>
          </MenuItem>
          <MenuItem component={<Link to="/Reserve" />}>
            <span style={{ marginRight: '10px' }}>
              رزرو غذا
            </span>
          </MenuItem>
          {/*         <MenuItem component={<Link to="/weeklymenu" />}>
          برنامه هفتگی سازمانی
        </MenuItem> */}
          <MenuItem component={<Link to="/ReviewOnBoofeh" />}>
            <span style={{ marginRight: '10px' }}>
              نظرسنجی بوفه ها
            </span>
          </MenuItem>
          <MenuItem component={<Link to="/myorgs" />}>
            <span style={{ marginRight: '10px' }}>
              درخواست های عضویت
            </span>
          </MenuItem>
          <MenuItem component={<Link to="/chooseOrg" />}>
            <span style={{ marginRight: '10px' }}>
              درخواست عضویت جدید
            </span>
          </MenuItem>
        </Menu>
      </Sidebar>
    );
  }

  const adminSidebar = () => {
    return (
      <Sidebar>
        <Menu
          menuItemStyles={{
            button: {
              ":hover": {
                color: "black",
                backgroundColor: "#EBE4D1"
              },
              color: "white",
            },
            root: {
              backgroundColor: "#26577C",
              opacity: "5",
            },
          }}
        >
          <MenuItem component={<Link to='/Updateorg' />}>
            تغییر اطلاعات سازمان
          </MenuItem>
          <MenuItem component={<Link to='/addweeklymenu' />}>
            منو هفتگی 
          </MenuItem>
          <MenuItem component={<Link to='/ListOfJoinRequests' />}>
            درخواست های عضویت
          </MenuItem>
          <MenuItem component={<Link to='/orgpage' />}>
            مدیریت بوفه ها
          </MenuItem>
          <MenuItem component={<Link to='/weeklymenu2' />}>
           مشاهده منو هفتگی بوفه ها 
          </MenuItem>
          
        </Menu>
      </Sidebar>
    );
  }

  return (
    <div>
      {AuthManager.isLoggedIn() && (
        flag === 1 ? adminSidebar() : userSidebar()
      )}
    </div>
  );
}
