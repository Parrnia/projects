import { useState, useEffect } from 'react';
import HomeOrgPage from '../HomePageForOrg/HomePage';
import ListOrg from '../listorg/listorg';
import AuthManager from '../APIs/AuthManager';
import Navbarparent from './navbarparent';
import styles from '../styles/bg.module.css'
const Home = () => {
    const [flag, setFlag] = useState(null);

    useEffect(() => {
        const checkUserType = async () => {
            const userType = await AuthManager.orguser();
            setFlag(userType);
        };

        checkUserType();
    }, []);

    if (flag === null) {
        return (
            <>
            <Navbarparent/>
            <div className='w-full flex items-center justify-center'>
                <h3 className="text-xl font-light text-gray-800 pt-3 pb-2 pr-3 text-right p-6 m-2"> در حال انتقال به صفحه اصلی... </h3>
            </div>
            </>
        );
    }

    return (
        <div>
          {AuthManager.isLoggedIn() && (flag === 1 ? <ListOrg /> : <ListOrg />)}
          {!AuthManager.isLoggedIn() && (
            <div className={styles.bg}>
          <Navbarparent/>
          <div style={{margin:'32rem'}}>
            
          </div>
          
          </div>
        )}
        </div>
      );
};
export default Home;

