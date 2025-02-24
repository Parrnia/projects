export const GlobalComponent = {
    // Api Calling
    API_URL : 'https://api-node.themesbrand.website/',
    headerToken : {'Authorization': `Bearer ${localStorage.getItem('token')}`},

    // Auth Api
    AUTH_API: 'http://172.20.40.57:5502/api/v1',
    FILES_URL: "http://172.20.40.57:5000/api/files/store",
    TEMPS_URL: "http://172.20.40.57:5000/api/files/temp",
    //AUTH_API:"http://localhost:5502/api/v1",
    //FILES_URL: "http://localhost:5000/api/files/store",
    //TEMPS_URL: "http://localhost:5000/api/files/temp",
     //AUTH_API:"https://auth.neginpart.com/api/v1",
     //FILES_URL: "https://neginpart.com/api/files/store",
     //TEMPS_URL: "https://neginpart.com/api/files/temp",

    // Products Api
    product:'apps/product',
    productDelete:'apps/product/',

    // Orders Api
    order:'apps/order',
    orderId:'apps/order/',

    // Customers Api
    customer:'apps/customer',


}