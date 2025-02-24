import axios from "axios";
import requests from "./AuthManager";

const GetAll = async () => {
  const token = requests.getToken();
  const baseUrl = "https://ghablameh.fiust.ir/api/v1/";
  let data = null;
  data = await axios.get(baseUrl + "notifications/", {
    headers: { Authorization: `JWT ${token}` },
  });
  // if (data === null || data.data.length == 0 || data.data == undefined) {
  //   data.data = [{id : 0 , title:'' , message: '' , created_at:'' , read:true , user:''}]
  // }
  // console.log("SUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU")
  // console.log(data.data)
  // console.log("SUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU",data.data)
  return {data : data.data , count : data.data.filter(m=>m.read == false).length};
};

export default { GetAll };
