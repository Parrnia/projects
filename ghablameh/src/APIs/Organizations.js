import axios from "axios";
import requests from './AuthManager';

const GetMyOrganizations = async () => {
    const baseUrl = "https://ghablameh.fiust.ir/api/v1/";
    let data = null;
    let token = requests.getToken();
    await axios.get(baseUrl + "clients/join-requests/",{headers: {'Authorization' : `JWT ${token}`}}).then(resp => data = resp.data);
    if(data == null) 
        data = [ {id:0,organization_name: '',created_at:'',status:''}]
    return data;
};

const JoinOrganization = async (OrganizationId) => {
    const baseUrl = "https://ghablameh.fiust.ir/api/v1/";
    const token = requests.getToken(); // { headers: { Authorization: "jwt " + token }
    let status;
    await axios.post(baseUrl + "clients/join-requests/",{organization:OrganizationId},{headers:{Authorization: `JWT ${token}`}}).then(resp => status = resp.status);
    if(status === 200 || status === 201)
        return true;
    else 
        return false;
}

const Top5Buffets = async () => {
    const baseUrl = "https://ghablameh.fiust.ir/api/v1/";
    const token = requests.getToken();
    let data = null;
    data = await axios.get(baseUrl + "buffets/top5/" , {headers:{Authorization:`JWT ${token}`}});
    return data.data;
}

const GetOrganizationBuffets = async () => {
    const baseUrl = "https://ghablameh.fiust.ir/api/v1/";
    const token = requests.getToken();
    let data = null;
    data = await axios.get(baseUrl + "buffets/" , {headers:{Authorization:`JWT ${token}`}});
    return data.data;
}
export default {GetMyOrganizations , JoinOrganization , Top5Buffets , GetOrganizationBuffets};