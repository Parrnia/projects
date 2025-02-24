// src/index.js

import React from 'react';
import './index.css'; // Ensure this line is present
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import LandingPage from "./Pages/Landing/landing";
import UserCourses from "./Pages/UserCourses/UserCourses";
import Courses from "./Pages/Courses/Courses";
import InviteFriends from "./Pages/InviteFriends/InviteFriends"; // Import the App component
import Profile from "././Pages/Profile/Profile"
import App from "./App";
// Render the App component into the root element
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(

        <App />
);
