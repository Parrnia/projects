import { useState, useEffect } from 'react';
import styles from '../styles/wallet.module.css';
import PropTypes from 'prop-types';
import axios from "axios";
import AuthManager from "../APIs/AuthManager";
// const UserWallet = (props) => {
const UserWallet = ({ open, setOpen }) => {
    // const [open, setOpen] = useState(props.open);
    const [fetchedAmount, setFetchedAmount] = useState('0');
    const [amount, setAmount] = useState('0');
    const [amountError, setAmountError] = useState('');
    const [requestError, setRequestError] = useState('');
    const [isWaiting, setIsWaiting] = useState(false);

    // fetch data
    useEffect(() => {
        const fetchUserData = async () => {
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
        if (AuthManager.isLoggedIn()) fetchUserData();
    }, [open]);

    //send data
    const handleTopUp = () => {
        if (!amount || amount === '0') {
            setAmountError(' مبلغ دلخواه خود را وارد کنید. ');
            return;
        } else if (amount === '0' || parseInt(amount) < 1000) {
            setAmountError(' مبلغ وارد شده باید بیشتر از ۱۰۰۰ تومان باشد. ')
        } else {
            postrequest();
            setAmountError('');
        }
    };


    //post request
    const postrequest = async () => {
        setRequestError('');
        setIsWaiting(true);
        const token = "JWT " + AuthManager.getToken();

        const requestdata = {
            amount: parseInt(amount)
        }

        try {
            const response = await axios.post('https://ghablameh.fiust.ir/api/v1/wallets/deposit/', requestdata, {
                headers: {
                    'Authorization': token,
                }
            });

            const redirectionUrl = response.data.pgw_url + '?token=' + response.data.token;

            // redirect
            window.location.href = redirectionUrl;

            setAmount('')
            setRequestError('')
            setRequestError('')
            setIsWaiting(false);
            setOpen(false);

        } catch (error) {
            // console.error('Error sending request: ',error);
            setIsWaiting(false);
            setRequestError(' مشکلی پیش آمده، لطفا در زمان دیگری امتحان کنید. ');
        }
    };

    //error
    const handleAmountChange = (e) => {
        const value = e.target.value;
        if (/^\d*$/.test(e.target.value)) {
            setAmountError('');
            setAmount(value);
        } else {
            setAmountError('مقدار دلخواه خود را به عدد و انگلیسی وارد کنید');
        }
    };

    const handlePresetAmount = (presetAmount) => {
        setAmount(presetAmount);
    };

    const walletModal = () => (
        <div className="inset-0 z-50" >

            {AuthManager.isLoggedIn() && open &&
                <div className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
                    <div className="bg-white p-10 px-14 rounded-lg shadow-lg items-center justify-center">
                        <div className=' mb-9 mt-4'>
                            <h1 className="flex text-lg font-bold justify-center" style={{ fontSize: '1.6rem' }}>
                                کیف پول
                            </h1>
                            <hr style={{ margin: '8px 0', border: '0.5px solid rgb(38, 87, 124)' }} />

                        </div>
                        <div className='flex flex-row w-full items-center justify-between'>
                            <h3 className="px-3">موجودی فعلی :</h3>
                            <div className={`${styles.bgmee} text-white py-1 px-3 rounded-lg`} style={{ fontSize: '1rem' }}>
                                {fetchedAmount} تومان
                            </div>
                        </div>
                        <div className='flex justify-center flex-col mb-9 mt-4'>
                            <label className={`${styles.label} mt-6 mb-2`} style={{ fontSize: '1.6rem' }}>
                                افزایش اعتبار
                            </label>
                            <hr style={{ margin: '8px 0', border: '0.5px solid #ccc' }} />

                        </div>

                        <div className='flex flex-row justify-center mb-7'>
                            <div className='px-1' onClick={() => handlePresetAmount('20000')}><div className={`${styles.bgme} text-white py-2 px-3 rounded-lg flex flex-col justify-center items-center`}> 20,000 تومان </div></div>
                            <div className='px-1' onClick={() => handlePresetAmount('40000')}><div className={`${styles.bgme} text-white py-2 px-3 rounded-lg flex flex-col justify-center items-center`}> 40,000 تومان </div></div>
                            <div className='px-1' onClick={() => handlePresetAmount('60000')}><div className={`${styles.bgme} text-white py-2 px-3 rounded-lg flex flex-col justify-center items-center`}> 60,000 تومان </div></div>
                        </div>

                        <div className='flex justify-center flex-col mt-8 mb-9 items-center'>
                            <input
                                placeholder=' مبلغ دلخواه '
                                value={amount !== '0' ? amount : ''}
                                // type="text"
                                onChange={handleAmountChange}
                                className={`${styles.input} mb-1`}
                                style={{ width: '17vw', textAlign: 'center' }}
                            />
                            {amountError && <span className='text-sm mt-2' style={{ color: 'red' }}>{amountError}</span>}
                        </div>
                        <div className='flex flex-col justify-center items-center'>
                            <div className='flex flex-row justify-center items-center'>

                            {!isWaiting && (
                                    <button onClick={handleTopUp} className={`${styles.button1} mx-2 text-white py-2 px-4 rounded mr-2`}>
                                        افزایش موجودی
                                    </button>
                                )}
                                {isWaiting && (
                                    <button onClick={handleTopUp} className={`${styles.button1} mx-2 text-white py-2 px-4 rounded mr-2`}>
                                        <div className={`${styles.spinner2}`}></div>
                                    </button>
                                )}

                                <button onClick={() => { setOpen(false), setAmountError(''), setAmount(''), setRequestError('') }} className={`${styles.button2} bg-gray-500 hover:bg-gray-700 text-white py-2 px-4 rounded`}>
                                    انصراف
                                </button>
                            </div>
                            {requestError && <span className='text-sm mt-4' style={{ color: 'red' }}>{requestError}</span>}
                        </div>

                    </div>
                </div>
            }
        </div>
    );

    return (
        <div>
            <div className={`${[styles.bg]} flex flex-col justify-center items-center`}>
                {walletModal()}
            </div>
            {/* <ToastContainer /> */}
        </div>
    );
};

// UserWallet.propTypes = {
//     open: PropTypes.bool.isRequired,
//     setOpen: PropTypes.func.isRequired,
// };


export default UserWallet;