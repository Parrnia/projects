import styles from "./EmailVerify.module.css"
import axios from 'axios';
import { Link, redirect } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import Navbarparent from "../components/navbarparent";
import Footer from "../components/footer";
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import '../styles/customNotifications.css';
import styless from '../styles/wallet.module.css'

function EmailVerify() {


    const [isWaiting, setIsWaiting] = useState(false);

    const createNotification = (type, data) => {
        return () => {
            switch (type) {
                case 'Error':
                    NotificationManager.error(`${data}`, '', 3000);
                    break;
                case 'Success':
                    NotificationManager.error(`${data}`, '', 3000);
                    break;
                case 'Notif':
                    NotificationManager.info(`${data}`, 3000, () => { navigate("/") });
                    break;

                default:
                    break;
            }
        };
    };

    const Error = (data) => {
        toast.warn(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{data}</div>
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
    const Notif = (data) => {
        toast.info(
            <div className="flex flex-col items-center">
                <div className="text-center mb-4">{data}</div>
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

    const navigate = useNavigate();
    const Resend = async () => {
        ///verification/resend/
        try {
            const formattedData = {
                email: localStorage.getItem('emailtoverify'),
            }
            const response = await axios.post('https://ghablameh.fiust.ir/api/v1/verification/resend/', formattedData);
            // Notif(' کد مجددا برای شما ارسال شد!')
            createNotification('Notif', ' کد مجددا برای شما ارسال شد!')();

        } catch (error) {
            // console.error('Error sending data:', error);
            //    Error("اکانت درست نیست یا اکانت شما قبلا تایید شده!");
            createNotification('Error', "اکانت درست نیست یا اکانت شما قبلا تایید شده!")();
        }
    }
    const ChecVerfiCode = async (e) => {
        //console.log(e)
        setIsWaiting(true);
        try {
            const formattedData = {
                code: e,
                email: localStorage.getItem('emailtoverify'),
            }
            //console.log(e)
            //console.log(localStorage.getItem('emailtoverify'))
            await axios.post('https://ghablameh.fiust.ir/api/v1/verification/verify_email/', formattedData);
            localStorage.setItem('emailtoverify', '');
            //const accessToken = response.data.tokens.access;
            //localStorage.setItem('token', accessToken);
            // alert("اکانت شما با موفقیت ثبت شد!");
            setIsWaiting(false);

            createNotification('Success', "اکانت شما با موفقیت ثبت شد!")();
            navigate("/login");
        } catch (error) {
            //console.error('Error sending data:', error);
            Error("کد تاییدی درست نیست یا اکانت شما قبلا تایید شده!");
            createNotification('Error', "کد تاییدی درست نیست یا اکانت شما قبلا تایید شده!")();
        }

    };
    return (
        <>
            <Navbarparent />
            <NotificationContainer />
            <div className={styles.container + ' ' + "grid grid-cols-7 grid-rows-7"}>
                <div className="col-start-2 col-span-5 row-start-2 row-span-5 bg-white h-full bg-opacity-60 rounded-lg m-5 grid grid-rows-5 grid-cols-5">
                    <div className='lg:items-center lg:col-start-4 lg:col-span-3 lg:row-start-2 lg:row-span-3 col-start-2 col-span-3'>
                        <img className="my-5" src='/thankyoudribble.gif'></img>
                    </div>
                    <div className="lg:col-start-2 lg:col-span-2 lg:row-start-2 text-center lg:mt-24 md:mt-24 mt-28 md:col-start-2 md:col-span-3 md:row-start-3 col-start-1 col-span-5 row-start-2 m-5 text-lg font-bold">
                        <div class="max-w-sm space-y-3">
                            <h2 classNmae="lg:scale-100 md:scale-100 scale-75 ">برای تایید ایمیل خود کد ۵ رقمی ارسالی را وارد کنید!</h2>
                            <input type="text" id="Code" maxlength="5" class="lg:scale-100 md:scale-100 scale-75 text-center py-2 lg:w-48 md:w-48 w-32 inline border-template-custom-blue rounded-lg text-sm focus:border-template-custom-orange focus:ring-template-custom-orange disabled:opacity-150 disabled:pointer-events-none dark:bg-neutral-900 dark:border-neutral-700 dark:text-neutral-400 dark:placeholder-neutral-500 dark:focus:ring-neutral-600" />
                            <br />
                            {!isWaiting && (
                                <button onClick={() => ChecVerfiCode(document.getElementById('Code').value)} className={`${styless.button1} ${styles.button_sign} text-center text-lg  lg:scale-90 md:scale-90 scale-75`}>
                                    ثبت کد
                                </button>
                            )}
                            {isWaiting && (
                                <button className={`} ${styles.button_sign} text-center  lg:scale-90 md:scale-90 scale-75`}>
                                    <div className={`${styless.spinner2}`}></div>
                                </button>
                            )}

                            {/* <button onClick={() => ChecVerfiCode(document.getElementById('Code').value)} className={styles.button_sign + " " + "text-center  lg:scale-90 md:scale-90 scale-75"}>ثبت کد</button> */}
                            <br />
                            <button onClick={() => Resend()} className="dfad text-template-custom-blue hover:text-template-custom-orange">ارسال مجدد</button>
                        </div>
                    </div>
                </div>
                {/* <ToastContainer /> */}
            </div>
        </>
    )
}
export default EmailVerify;