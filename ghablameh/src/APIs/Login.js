import axios from "axios";

const isLoggedIn = () => 
{
    let tkoen = localStorage.getItem("token");
    if(tkoen === null) 
    {
        return false;
    } else {
        return {
            token : token,
            isLoggedin : true
        };
    }
}

const LoginRequest = (username , password) => {
    const baseurl = "https://ghablameh.fiust.ir/api/v1";
    let data = null;
    axios.post(baseurl+"/auth/login/" , {username :username , password : password}).then(resp => data = resp.data);
    return data;
}
export default {isLoggedIn , LoginRequest};