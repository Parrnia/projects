import axios from 'axios/dist/node/axios.cjs';

const Api = axios.create({
    baseURL: 'https://backenddotnet.boursiko.ir/',
    headers: {
        'Content-Type': 'application/json',
    },
});

describe('API Configuration', () => {
    test('should have correct base URL', () => {
        expect(Api.defaults.baseURL).toBe('https://backenddotnet.boursiko.ir/');
    });

    test('should have correct content type header', () => {
        expect(Api.defaults.headers['Content-Type']).toBe('application/json');
    });

    test('should be an axios instance', () => {
        expect(Api).toHaveProperty('request');
        expect(Api).toHaveProperty('get');
        expect(Api).toHaveProperty('post');
        expect(Api).toHaveProperty('put');
        expect(Api).toHaveProperty('delete');
    });
});

export default Api;