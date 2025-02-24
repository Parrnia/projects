import './App.css';
import Friend from './Pages/Firend/Friend';
import Landing from './Pages/Landing/landing';
import Profile from './Pages/Profile/Profile';
import Course from './Pages/Courses/Courses';
import Courses from './Pages/UserCourses/UserCourses';
import Azmon from './Pages/azmon/azmon';
// import Add_Exam from './Pages/Admin_Panel/Layout/Exam/Add_Exam';
import Show_Exams from './Pages/Admin_Panel/Layout/Exam/Show_Exams';
import Add_Course from './Pages/Admin_Panel/Layout/Course/Add_Course';
import Show_Courses from './Pages/Admin_Panel/Layout/Course/Show_Courses';
import Azmon1 from './Pages/azmon/azmon1';
import  Final from './Pages/azmon/final';

import { BrowserRouter, Route, Routes } from "react-router-dom";


function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Landing />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/searchfriend" element={<Friend />} />
        <Route path="/courses" element={<Courses />} />
        <Route path="/course" element={<Course />} />
        <Route path="/azmon" element={<Azmon />} />
        <Route path="/azmon1" element={<Azmon1 />} />
        <Route path="/final" element={<Final />} />
        {/* <Route path="/adminpanel/addexam" element={<Add_Exam />} /> */}
        <Route path="/adminpanel/exams" element={<Show_Exams />} />
        <Route path="/adminpanel/addcourse" element={<Add_Course />} />
        <Route path="/adminpanel/courses" element={<Show_Courses />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;