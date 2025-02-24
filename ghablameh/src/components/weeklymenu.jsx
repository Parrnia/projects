import { useState } from 'react';
import styles from  '../styles/weeklymenu.module.css';
import Navbarparent from './navbarparent';

const weeklyMenuData = [
{ روز: 'دوشنبه', وعده: 'صبحانه', غذا: 'تست', تاریخ: '2024-04-14', زمان_توزیع: '8:00 ', هزینه:50000   },
{ روز: 'دوشنبه', وعده: 'صبحانه', غذا: 'تست', تاریخ: '2024-04-14', زمان_توزیع: '8:00 ', هزینه: 50000},
{ روز: 'دوشنبه', وعده: 'ناهار', غذا: 'سالاد', تاریخ: '2024-04-14', زمان_توزیع: '12:00 ', هزینه: 80000 },
{ روز: 'سه‌شنبه', وعده: 'صبحانه', غذا: 'صبحانه', تاریخ: '2024-04-15', زمان_توزیع: '8:00 ', هزینه: 40000 },
{ روز: 'سه‌شنبه', وعده: 'ناهار', غذا: 'ساندویچ', تاریخ: '2024-04-15', زمان_توزیع: '12:00 ', هزینه: 60000 },

{ روز: 'چهارشنبه', وعده: 'صبحانه', غذا: 'پنکیک', تاریخ: '2024-04-16', زمان_توزیع: '8:00 ', هزینه: 70000 },
{ روز: 'چهارشنبه', وعده: 'ناهار', غذا: 'برگر', تاریخ: '2024-04-16', زمان_توزیع: '12:00 ', هزینه: 90000 },
{ روز: 'پنج‌شنبه', وعده: 'صبحانه', غذا: 'املت', تاریخ: '2024-04-17', زمان_توزیع: '8:00 ', هزینه: 60000 },
{ روز: 'پنج‌شنبه', وعده: 'ناهار', غذا: 'پیتزا', تاریخ: '2024-04-17', زمان_توزیع: '12:00 ', هزینه: 10000 },
{ روز: 'جمعه', وعده: 'صبحانه', غذا: 'ماست', تاریخ: '2024-04-18', زمان_توزیع: '8:00 ', هزینه: 30000 },
{ روز: 'جمعه', وعده: 'ناهار', غذا: 'سوشی', تاریخ: '2024-04-18', زمان_توزیع: '12:00 ', هزینه: 120000 },
{ روز: 'شنبه', وعده: 'صبحانه', غذا: '', تاریخ: '2024-04-19', زمان_توزیع: '8:00 ', هزینه: 40000 },
{ روز: 'شنبه', وعده: 'ناهار', غذا: 'تاکو', تاریخ: '2024-04-19', زمان_توزیع: '12:00 ', هزینه: 80000 },
{ روز: 'یکشنبه', وعده: 'صبحانه', غذا: 'وافل', تاریخ: '2024-04-20', زمان_توزیع: '8:00 ', هزینه: 60000 },

];

const WeeklyMenu = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  const totalPages = Math.ceil(weeklyMenuData.length / itemsPerPage);
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = weeklyMenuData.slice(indexOfFirstItem, indexOfLastItem);

  const handlePageChange = (pageNum) => {
    setCurrentPage(pageNum);
  };

  const renderHeader = () => {
    const headerValues = ['روز', 'وعده', 'غذا', 'تاریخ', 'زمان توزیع', 'هزینه'];
    return headerValues.map((value, index) => (
      <th key={index}>{value}</th>
    ));
  };

  const renderBody = () => {
    return currentItems.map(({ روز: day,وعده : meal,   غذا : food,تاریخ : date,  زمان_توزیع :distributionTime, هزینه :cost }, index) => (
      <tr key={index}>
        <td>{day}</td>
        <td>{meal}</td>
        <td>{food}</td>
        <td>{date}</td>
        <td>{distributionTime}</td>
        <td>{cost}</td>
      </tr>
    ));
  };

  const renderPagination = () => {
    const pageNumbers = [];
    for (let i = 1; i <= totalPages; i++) {
      pageNumbers.push(
        <li key={i} className={`${styles.pageItem} ${currentPage === i ? styles.active : ''}`}>
          <button className={styles.pageLink} onClick={() => handlePageChange(i)}>
            {i}
          </button>
        </li>
      );
    }
    
    return (
      <nav>
        <ul className={styles.pagination}>
          <li className={`${styles.pageItem} ${currentPage === 1 ? styles.disabled : ''}`}>
            <button className={`${styles.pageLink} ${styles.previousPage}`} onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>
              قبلی
            </button>
          </li>
          {pageNumbers}
          <li className={`${styles.pageItem} ${currentPage === totalPages ? styles.disabled : ''}`}>
            <button className={`${styles.pageLink} ${styles.nextPage}`} onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === totalPages}>
              بعدی
            </button>
          </li>
        </ul>
      </nav>
    );
  };
  return (
    <>
     <div className={styles.app}>
      <Navbarparent/>
    <p id={styles.title}>منوی هفتگی</p>
      <div className={styles.menu}>
    <table id={styles.employee}>
      <thead>
        <tr>{renderHeader()}</tr>
      </thead>
      <tbody>{renderBody()}</tbody>
    </table>
  </div>
  {renderPagination()}
  </div>

   
    </>
  );
};

export default WeeklyMenu;