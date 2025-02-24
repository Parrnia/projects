import ChooseOrganization from "../components/ChooseOrganization";
import Navbarparent from "../components/navbarparent";
import styles from '../styles/bg.module.css'
const ChooseOrganizationPage = () => {

    return (
      <div dir="rtl">
        <Navbarparent/>
          <div className={`${styles['bg']}`}>
          
          <div><ChooseOrganization/></div>
            
          </div>
      </div>
    );
}

export default ChooseOrganizationPage;