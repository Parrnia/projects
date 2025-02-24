import axios from 'axios';

const Api = axios.create({
    baseURL: 'https://backenddotnet.boursiko.ir/',
});

export default Api; 