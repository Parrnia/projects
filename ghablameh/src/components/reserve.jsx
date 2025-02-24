import { useRef, useState, useEffect } from "react";
import Navbarparent from './navbarparent'
import AuthManager from "../APIs/AuthManager";
import axios from "axios";
import { useMemo } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Organizations from "../APIs/Organizations";
import jalaliMoment from 'jalali-moment';
import moment from 'moment';
import Select from 'react-select';
import { Link } from "react-router-dom";
import useMediaQuery from '@mui/material/useMediaQuery';
import styles from '../styles/wallet.module.css'
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css'; 
import '../styles/customNotifications.css';
const Reserve = () => {
    const [fetchedData, setFetchedData] = useState([])
    const [data, setData] = useState([]);
    const [reservedfoods, setReservedFoods] = useState([]);
    const [fetchedAmount, setFetchedAmount] = useState();
    const [loading, setLoading] = useState(true);
    //const [fromDate, setFromDate] = useState();
    //const [toDate, setToDate] = useState();
    const currentBuffet = useRef(null);
    const fromDate = useRef(0);
    const toDate = useRef(0);

    const [isLoading, setIsLoading] = useState(false);

    const isBigScreen = useMediaQuery('(min-width: 470px)')


    //fetch dates
    useEffect(() => {
        jalaliMoment.locale('fa', {
            week: {
                dow: 6,
            },
        });

        const getFirstDayOfWeek = () => {
            const now = jalaliMoment();
            // console.log(now)
            const startOfWeek = now.startOf('week');
            return startOfWeek.format('jYYYY/jM/jD');
        };
        const getEndDayOfWeek = () => {
            const now = jalaliMoment();
            const endweek = now.endOf('week');
            return endweek.format('jYYYY/jM/jD');
        };
        function convertToChristian(date) {
            const jalaliDate = jalaliMoment(date, 'jYYYY/jM/jD');
            const christianDate = jalaliDate.toDate();
            const formattedDate = moment(christianDate, 'ddd MMM DD YYYY').format('YYYY-MM-DD');
            return formattedDate;
        }

        const firstDayOfWeek = getFirstDayOfWeek();
        const endweek = getEndDayOfWeek();

        const christianDatefday = convertToChristian(firstDayOfWeek);
        fromDate.current = christianDatefday;
        const christianDatelday = convertToChristian(endweek);
        toDate.current = christianDatelday;
        // console.log('line 55')
    }, []);

    async function getNextWeek() {
        try {
            const currentDate = new Date(fromDate.current);
            currentDate.setDate(currentDate.getDate() + 7);
            fromDate.current = currentDate.toISOString().split('T')[0]; // Format as 'YYYY-MM-DD'

            const currentDate2 = new Date(toDate.current);
            currentDate2.setDate(currentDate2.getDate() + 7);
            toDate.current = currentDate2.toISOString().split('T')[0];

            // Assuming fetchData() is an asynchronous function
            await fetchData();

            // console.log('Next week:');
            // console.log('From Date:' + fromDate.current);
            // console.log("To Date : " + toDate.current)
            // console.log(fetchedData);
            // console.log("=======================================")
            // console.log('To Date:', toDate);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }

    async function getLastWeek() {
        try {
            const currentDate = new Date(fromDate.current);
            currentDate.setDate(currentDate.getDate() - 7);
            fromDate.current = currentDate.toISOString().split('T')[0];

            const currentDate2 = new Date(toDate.current);
            currentDate2.setDate(currentDate2.getDate() - 7);
            toDate.current = currentDate2.toISOString().split('T')[0];

            // Assuming fetchData() is an asynchronous function
            await fetchData();

            // console.log('Last week:');
            // console.log('From Date:', fromDate.current);
            // console.log('To Date:', toDate);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }

    //fetch buffets
    useEffect(() => {
        const fetchData = async () => {
            try {
                let result = await Organizations.GetOrganizationBuffets();
                setData(result);
                // console.log(result)
            } catch (error) {
                console.error("Error fetching data:", error);
            } finally {
                setLoading(false);
            }
        };

        if (AuthManager.isLoggedIn()) {
            fetchData();
        }
    }, []);

    //fetch table data
    const fetchData = async () => {
        // setFetchedData([]);
        setIsLoading(true);
        // console.log('xdfcgvhmbj,nklkjhvgcxzfgnvmhj,kkhgfxcghjkl')
        try {
            if (AuthManager.isLoggedIn()) {
                const token = AuthManager.getToken();
                const buffet_id = parseInt(currentBuffet.current.value);
                // console.log("From Date From req : " + fromDate.current)
                // console.log("To Date From req : " + toDate.current)
                const response = await axios.get(
                    `https://ghablameh.fiust.ir/api/v1/buffets/${buffet_id}/weekly-menus/?from_date=${fromDate.current}&to_date=${toDate.current}`,
                    { headers: { Authorization: "JWT " + token } }
                );

                setFetchedData(response.data);
                fetchReservations();
                setIsLoading(false);
            }
        } catch (error) {
            console.error("Error fetching data: ", error);
            setIsLoading(false);
        }
        setIsLoading(false);
    };

    // useEffect(() => {
    //     if (AuthManager.isLoggedIn()) {
    //         fetchData();
    //     }
    // }, [currentBuffet.current]);

    // fetch reserved foods

    const fetchReservations = async () => {
        try {
            const token = AuthManager.getToken();

            const response = await axios.get(`https://ghablameh.fiust.ir/api/v1/reserve/?from_date=${fromDate.current}&to_date=${toDate.current}`,
                { headers: { Authorization: "JWT " + token } }
            );
            setReservedFoods(response.data);
        } catch (error) {
            console.error("Error fetching reservations: ", error);
        }
    };

    //fetch every 5 seconds
    useEffect(() => {
        if (currentBuffet.current !== null) {
            // const fetchReservationsId = setInterval(() => {
            //     fetchReservations();
            // }, 5000); // 5 seconds
            // const fetchDataId = setInterval(() => {
            //     fetchData();
            // }, 5000); // 5 seconds

            // return () => {
            //     clearInterval(fetchReservationsId);
            //     // clearInterval(fetchDataId);
            // };

        }
    }, []);

    //fetch wallet data
    const fetchWalletData = async () => {
        try {
            const token = AuthManager.getToken();

            const response = await axios.get("https://ghablameh.fiust.ir/api/v1/wallets/me/",
                { headers: { Authorization: "JWT " + token } }
            );
            setFetchedAmount(response.data.balance);
        } catch (error) {
            console.error("Error fetching user data: ", error);
        }
    };

    // useEffect(() => {
    //     const intervalId = AuthManager.isLoggedIn() ? setInterval(fetchWalletData, 5000) : null;

    //     return () => {
    //         if (intervalId !== null) {
    //             clearInterval(intervalId);
    //         }
    //     };
    // }, []);

    // useEffect(()=> {
    //     alert("Current Value of ToDate : " +toDate)
    // },[toDate]);

    //reserve
    const handlereserve = async (food) => {

        const token = 'JWT ' + localStorage.getItem("token");
        const url = 'https://ghablameh.fiust.ir/api/v1/reserve/';

        const requestData = {
            meal_food: food.mealFoodId
        };

        try {
            const response = await axios.post(url, requestData, {
                headers: {
                    'Authorization': token,
                }
            });

            if (response.status === 201) {
                fetchReservations();
                // checkToast(food);
                createNotification('reserved',food)();
            } else {
                console.log('Reservation update failed:', response.data);
                fetchData();
                createNotification('fail', food)();
            }
        } catch (error) {
            console.error('An error occurred:', error);
            // failToast();
            
            if(error.response.data.non_field_errors){
                createNotification('failreserve', error.response.data.non_field_errors)();
                return;
            }
            createNotification('fail', food)();
        }
    }

    //delete reservation
    const handleDeletereserve = async (food) => {
        const reservedFood = reservedfoods.find(item => item.meal_food.food.id === parseInt(food.foodId));
        const token = 'JWT ' + localStorage.getItem("token");
        const url = 'https://ghablameh.fiust.ir/api/v1/reserve/' + reservedFood.id + '/';

        try {
            const response = await axios.delete(url, {
                headers: {
                    'Authorization': token,
                }
            });

            if (response.status == 204) {
                // uncheckedToast(food);
                fetchData();
                createNotification('unreserved',food)();
                fetchReservations();
            } else {
                // console.log('Deleting reservation failed:', response.data);
                // failToast();
                createNotification('fail',food)();
            }
        } catch (error) {
            // console.error('An error occurred:', error);
            // failToast();
            createNotification('fail',food)();
        }
    }
    

    const createNotification = (type, food) => {
        return () => {
            switch (type) {
                case 'reserved':
                    NotificationManager.success(`${food.name} رزرو شد`,'', 2000);
                    break;
                case 'unreserved':
                    NotificationManager.success(`رزرو ${food.name} حذف شد`,'', 2000);
                    break;
                case 'numberinstock':
                    NotificationManager.warning('موجودی غذا کافی نیست ','', 2000);
                    break;
                case 'fail':
                    NotificationManager.error('در فرایند رزرو خطایی رخ داد ','', 2000);
                    break;
                case 'wallet':
                    NotificationManager.warning('کیف پول خود را شارژ کنید ','', 2000);
                    break;
                    case 'failreserve':
                        NotificationManager.error(`${food}`,'', 3000);
                        break;

                default:
                    break;
            }
        };
    };
    // reservation completed toast
    const checkToast = (food) => {
        toast.info(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{`${food.name} رزرو شد`}</div>
            </div>,
            {
                position: 'top-center',
                autoClose: 3000,
                closeButton: true,
                hideProgressBar: false,
                progress: undefined,
                icon: false,
            }
        );
    };

    // reservation deleted toast
    const uncheckedToast = (food) => {
        toast.warn(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{` رزرو ${food.name}  حذف شد`}</div>
            </div>,
            {
                position: 'top-center',
                autoClose: 3000,
                closeButton: true,
                hideProgressBar: false,
                progress: undefined,
                icon: true,
            }
        );
    };

    // reservation failed toast
    const failToast = () => {
        toast.info(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{`!در فرایند رزرو خطایی رخ داد `}</div>
            </div>,
            {
                position: 'top-center',
                autoClose: 3000,
                closeButton: true,
                hideProgressBar: false,
                progress: undefined,
                icon: false,
            }
        );
    };

    // number in stock toast
    const numberInStockToast = () => {
        toast.info(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{` !موجودی غذا کافی نیست `}</div>
            </div>,
            {
                position: 'top-center',
                autoClose: 3000,
                closeButton: true,
                hideProgressBar: false,
                progress: undefined,
                icon: false,
            }
        );
    };

    // charge wallet toast
    const chargeWalletToast = () => {
        toast.info(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{` کیف پول خود را شارژ کنید `}</div>
            </div>,
            {
                position: 'top-center',
                autoClose: 3000,
                closeButton: true,
                hideProgressBar: false,
                progress: undefined,
                icon: false,
            }
        );
    };

    // food Checkbox change handler
    const handleCheckboxChange = (food, isChecked) => {
        const amount = parseInt(fetchedAmount, 10);
        // console.log(food)
        if (isChecked) {
            if (food.price > amount) {
                // chargeWalletToast();
                createNotification('wallet',food)();
                // console.log(food.price,amount)
            }
            else if (food.numberInStock == 0) {
                createNotification('numberinstock',food)();
            }
            else {
                handlereserve(food);
            }
        } else {
            handleDeletereserve(food);
        }
    };

    const convertToJalali = (date) => {
        const today = new Date(date).toLocaleDateString('fa-IR');
        return today;
    };

    //render table
    const TableComponent = ({ data }) => {
        setIsLoading(true);
        const dates = [...new Set(data.map(entry => entry.date))].sort((a, b) => new Date(a) - new Date(b));
        // console.log(dates)
        const mealNames = [...new Set(data.flatMap(entry => entry.meals.map(meal => meal.name)))];

        //sort meals
        const priorityOrder = ['صبحانه', 'ناهار', 'شام'];
        mealNames.sort((a, b) => {
            const priorityA = priorityOrder.indexOf(a);
            const priorityB = priorityOrder.indexOf(b);
            if (priorityA !== -1 && priorityB !== -1) {
                return priorityA - priorityB;
            } else if (priorityA !== -1) {
                return -1;
            } else if (priorityB !== -1) {
                return 1;
            } else {
                return a.localeCompare(b);
            }
        });

        const organizedData = useMemo(() => {
            return dates.map(date => {
                const dateEntry = { date };
                const mealsForDate = data.find(entry => entry.date === date)?.meals || [];

                mealNames.forEach(mealName => {
                    const meal = mealsForDate.find(m => m.name === mealName);

                    if (meal) {
                        // For each meal, create an array of items with detailed information
                        dateEntry[mealName] = meal.items.map(item => ({
                            name: item.food.name,
                            foodId: item.food.id,
                            mealFoodId: item.id, // Directly assigning the meal's ID as mealFoodId
                            price: item.price,
                            numberInStock: item.number_in_stock,
                        }));
                    } else {
                        dateEntry[mealName] = []; // If no meal is found, assign an empty array
                    }
                });

                return dateEntry;
            });
        }, [data, dates, mealNames]);

        setIsLoading(false);
        // console.log(organizedData)

        if (data.length === 0 && options.length !== 0 && dates.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center">

                    {/* last week / next week button */}
                    <div className="flex flex-row my-5 justify-center items-center bg-white bg-opacity-60 rounded-lg" style={{ padding: isBigScreen ? '16px' : '10px'}}>
                        <button className="rounded-lg bg-sky-800 px-5 py-2 text-white hover:bg-sky-900" onClick={getLastWeek} style={{ fontSize: isBigScreen ? '1rem' : '0.6rem', marginLeft: isBigScreen ? '40px' : '10px', paddingLeft: isBigScreen ? '1rem' : '0.5rem', paddingRight: isBigScreen ? '1rem' : '0.5rem' }}> هفته قبلی </button>
                        {/* currentBuffet.current.value */}
                        <div className=" flex items-center justify-center flex-row bg-opacity-50 py-1 px-2 rounded-lg" style={{ background: 'rgba(38, 87, 124, 0.2)', fontSize: isBigScreen ? '1rem' : '0.8rem' }}>
                            {convertToJalali(toDate.current)}
                            {/* {fromDate.current} */}
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            {isBigScreen && (<div>-&nbsp;&nbsp;&nbsp;&nbsp;</div>)}
                            {!isBigScreen && (<div>&nbsp;&nbsp;&nbsp;&nbsp;</div>)}

                            {convertToJalali(fromDate.current)}
                        </div>

                        <button className="rounded-lg bg-sky-800 px-5 py-2 text-white hover:bg-sky-900" onClick={getNextWeek} style={{ fontSize: isBigScreen ? '1rem' : '0.6rem', marginRight: isBigScreen ? '40px' : '10px', paddingLeft: isBigScreen ? '1rem' : '0.5rem', paddingRight: isBigScreen ? '1rem' : '0.5rem' }}>هفته بعدی </button>
                    </div>

                    <span className="flex flex-row justify-center my-20 items-center p-2 rounded-lg " style={{ background: 'rgba(38, 87, 124,0.2)' }} >  غذایی جهت رزرو وجود ندارد. </span>

                </div>
            );
        }

        //table
        if (data.length !== 0 && options.length !== 0) {
            return (
                <div className="">

                    <div className="flex flex-col items-center justify-center">
                        {/* last week / next week button */}
                        <div className="flex flex-row my-5 justify-center items-center bg-white bg-opacity-60 rounded-lg" style={{ padding: isBigScreen ? '16px' : '10px' }}>
                            <button className="rounded-lg bg-sky-800 px-5 py-2 text-white hover:bg-sky-900" onClick={getLastWeek} style={{ fontSize: isBigScreen ? '1rem' : '0.6rem', marginLeft: isBigScreen ? '40px' : '10px', paddingLeft: isBigScreen ? '1rem' : '0.5rem', paddingRight: isBigScreen ? '1rem' : '0.5rem' }}> هفته قبلی </button>
                            {/* currentBuffet.current.value */}
                            <div className=" flex items-center justify-center flex-row bg-opacity-50 py-1 px-2 rounded-lg" style={{ background: 'rgba(38, 87, 124, 0.2)', fontSize: isBigScreen ? '1rem' : '0.8rem' }}>
                                {convertToJalali(toDate.current)}
                                {/* {fromDate.current} */}
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                {isBigScreen && (<div>-&nbsp;&nbsp;&nbsp;&nbsp;</div>)}
                                {!isBigScreen && (<div>&nbsp;&nbsp;&nbsp;&nbsp;</div>)}

                                {convertToJalali(fromDate.current)}
                            </div>

                            <button className="rounded-lg bg-sky-800 px-5 py-2 text-white hover:bg-sky-900" onClick={getNextWeek} style={{ fontSize: isBigScreen ? '1rem' : '0.6rem', marginRight: isBigScreen ? '40px' : '10px', paddingLeft: isBigScreen ? '1rem' : '0.5rem', paddingRight: isBigScreen ? '1rem' : '0.5rem' }}>هفته بعدی </button>
                        </div>
                    </div>

                    {/* table */}
                    <div className="m-4">
                        <table className="min-w-full divide-y divide-gray-300 shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
                            <thead className="text-white" style={{ background: 'rgb(38, 87, 124)' }}>
                                <tr>
                                    <th className="w-1/5 p-2 text-lg font-medium tracking-wider text-center">روز</th>
                                    {mealNames.map((mealName, index) => (
                                        <th key={index} className="w-1/5 p-2 text-lg font-medium tracking-wider text-center">{mealName}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {organizedData.map((entry, rowIndex) => (
                                    <tr key={rowIndex} className={rowIndex % 2 === 0 ? 'bg-white' : 'bg-slate-50'} style={{border:'0.5px solid rgba(38, 87, 124, 0.2)'}}>
                                        <td className="p-2 text-center">{convertToJalali(entry.date)}</td>
                                        {mealNames.map((mealName, index) => (
                                            <td key={index} className="p-2">
                                                {entry[mealName].map((food, foodIndex) => (
                                                    <div key={foodIndex} className="flex items-center justify-center">
                                                        <div className="flex flex-row justify-center items-center p-1 rounded-lg my-2" style={{ background: 'rgba(38, 87, 124,0.3)' }}>
                                                            <input
                                                                type="checkbox"
                                                                checked={reservedfoods.some(reserved => reserved.meal_food.food.id === food.foodId)}
                                                                onChange={(e) => handleCheckboxChange(food, e.target.checked)}
                                                                className="m-3 rounded-sm"
                                                            />
                                                            <div className="flex flex-col p-2 items-start justify-center">
                                                                <span className="text-sm font-bold text-sky-900">{food.name}</span>
                                                                <span className="text-xs pt-1 font-normal ">{food.price} تومان </span>
                                                                <span className="text-xs "> موجودی: {food.numberInStock} عدد </span>
                                                                {/* <span className="text-sm text-gray-700">{food.foodId}  </span> */}
                                                            </div>
                                                        </div></div>
                                                ))}
                                            </td>
                                        ))}
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>

                </div>
            );
        }
    };

    const options = data.map((item) => ({
        value: item.id,
        label: item.name + ' - ' +item.organization_name
    }));

    return (
        <>
            <Navbarparent />
            <NotificationContainer/>
            <div style={{ width: "100%" }} className="px-5 py-3 my-24">
                <div className="grid grid-cols-3 my-4 text-center"></div>

                <div className="grid grid-cols-3 w-full" >
                    <div></div>

                    {/* choose buffet */}
                    <div className="mb-3 content-center w-full flex justify-center items-center">
                        {options.length === 0  && !isLoading ? (
                            <div className='flex flex-col w-full justify-center items-center'>
                                <p className='text-lg mb-7 w-full'> برای رزرو غذا ابتدا عضو سازمان ها شوید. </p>
                                <Link to='/chooseOrg' className='bg-sky-800 hover:bg-sky-900 text-white text-center rounded-lg py-2 px-2'> عضویت در سازمان ها </Link>
                            </div>
                        ) : (
                            <div className="mb-3 content-center w-full flex justify-center items-center">
                                <Select
                                    options={options}
                                    value={currentBuffet.current}
                                    // isLoading={loading}
                                    className="rounded w-60"
                                    placeholder=" بوفه خود را انتخاب کنید. "
                                    theme={(theme) => ({
                                        ...theme,
                                        colors: {
                                            ...theme.colors,
                                            text: 'de6016',
                                            primary: 'rgb(38, 87, 124)',
                                            primary25: 'rgba(38, 87, 124,0.4)',
                                        }
                                    })}
                                    // style={{padding: isBigScreen ? '16px':'10px' }}
                                    onChange={selectedOption => {
                                        setFetchedData([]);
                                        setIsLoading(true);
                                        currentBuffet.current = selectedOption;
                                        fetchData();
                                    }}
                                />
                            </div>
                        )}
                    </div>
                </div>

                {/* table */}
                {!isLoading && (<TableComponent data={fetchedData} />)}

                {isLoading && (
                    <div className="flex flex-col items-center my-10 justify-center">
                        <div className={`${styles.spinner}`}></div>
                        {/* <p className="text-center text-gray-500 m-9"> در حال دریافت اطلاعات...  </p> */}
                    </div>
                )}
            </div>

        </>
    );
};

export default Reserve;