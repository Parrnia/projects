import axios from "axios";
import { useState } from "react";
import organizations from "./Organizations";
const isLoggedIn = () => {
  let tkoen = localStorage.getItem("token");
  if (tkoen === null) {
    return false;
  } else {
    return true;
  }
};

const orguser = async () => {
  const token = getToken();
  const baseurl = "https://ghablameh.fiust.ir/api/v1";
  let user = true;
  let organization = true;

  try {
    await axios.get(baseurl + "/clients/me", { headers: { 'Authorization': "JWT " + token } });
  } catch (error) {
    if (error.response && error.response.status === 403) {
      user = false;
    }
  }

  try {
    await axios.get(baseurl + "/organizations/me", { headers: { 'Authorization': "JWT " + token } });
  } catch (error) {
    if (error.response && error.response.status === 403) {
      organization = false;
    }
  }

  if (!user && organization) {
    return 1; // organization
  } else if (user && !organization) {
    return 2; // user
  } else {
    return 3;
  }
};


const LoginRequest = async (username, password) => {
  const baseurl = "https://ghablameh.fiust.ir/api/v1";
  const body = { username: username, password: password };
    const data = await axios.post(baseurl + "/auth/login/", body).then(resp =>{ return resp}).catch(err => {return err});
    return data;


  // console.log(data.data)
  
};

const GetOrganizations = async () => {
  const baseurl = "https://ghablameh.fiust.ir/api/v1";
  const token = localStorage.getItem("token");
  let data = []; //data =
  await axios
    .get(baseurl + "/organizations", {
      headers: { Authorization: "JWT " + token },
    })
    .then((resp) => (data = resp.data))
    .catch(function (error) {
      if (error.response) {
        if (error.response.status === 401) {
          data.push({ id: 0, name: "ابتدا وارد حساب کاربری خود شوید!" });
        } else {
          data.push({ id: 0, name: "سازمانی یافت نشد!" });
        }
      } else if (error.request) {
        data.push({ id: 0, name: "سازمانی یافت نشد!" });
      } else {
        data.push({ id: 0, name: "سازمانی یافت نشد!" });
      }
    });
  return data;
};

const setToken = (token) => {
  localStorage.setItem("token", token);
};

const getToken = () => {
  return localStorage.getItem("token");
};
export default {
  isLoggedIn,
  LoginRequest,
  GetOrganizations,
  setToken,
  getToken,
  orguser,
};
