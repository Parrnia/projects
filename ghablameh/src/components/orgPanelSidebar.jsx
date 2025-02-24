// import { Sidebar, Menu, MenuItem, SubMenu } from "react-pro-sidebar";
// import { Link } from "react-router-dom";
// export default function DefaultSidebar() {
//   return (
//     <Sidebar className="">
//       <Menu
//         menuItemStyles={{
//           button: {
//             ":hover": {
//               color:"black",
//               backgroundColor: "#EBE4D1"
//             },
//             color: "white",
            
//           },
//           root: {
//             backgroundColor: "#26577C",
//             // opacity: "5",
//           },
//         }}
//       >
//         <MenuItem component={<Link to="/Updateorg" />}>
//         تغییر اطلاعات سازمان 
//         </MenuItem>
//         <MenuItem component={<Link to="/ListOfJoinRequests" />}>
//         درخواست های عضویت 
//         </MenuItem>
//         <MenuItem component={<Link to="/orgpage" />}>
//         مدیریت بوفه ها 
//         </MenuItem>

//       </Menu>
//     </Sidebar>
//   );
// }
import {
  Card,
  Typography,
  List,
  ListItem,
  ListItemPrefix,
  ListItemSuffix,
  Chip,
} from "@material-tailwind/react";
import { Link } from "react-router-dom";
// import {
//   PresentationChartBarIcon,
//   ShoppingBagIcon,
//   UserCircleIcon,
//   Cog6ToothIcon,
//   InboxIcon,
//   PowerIcon,
// } from "@heroicons/react/24/solid";
export default function OrgSidebar() {
  return (
    <Card className="h-[calc(100vh-2rem)] w-full max-w-[20rem] p-4 shadow-xl shadow-blue-gray-900/5">
      <div className="mb-2 p-4 flex justify-center">
        <Typography variant="h5" color="blue-gray">
          داشبورد
        </Typography>
      </div>
      <List>

        <ListItem>
          <ListItemPrefix>
            <Link to='/Updateorg'>تغییر اطلاعات سازمان</Link>
          </ListItemPrefix>
        </ListItem>

        <ListItem>
          <ListItemPrefix>
            <Link to='/ListOfJoinRequests'> درخواست های عضویت </Link>
          </ListItemPrefix>
        </ListItem>

        <ListItem>
          <ListItemPrefix>
            <Link to='/orgpage'> مدیریت بوفه ها </Link>
          </ListItemPrefix>
        </ListItem>
        
      </List>
    </Card>
  );
}