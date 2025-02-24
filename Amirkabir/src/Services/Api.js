const Api = process.env.NODE_ENV === 'test'
    ? require('./Api.test.js').default
    : require('./Api.prod.js').default;

export default Api;