import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Cards from './Cards';
import Boofeh from './Boofeh';
import styles from './Cards.module.css';
import { ToastContainer, toast } from 'react-toastify';
import Navbarparent from '../components/navbarparent';
import { Link } from 'react-router-dom';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import '../styles/customNotifications.css';

function DataFromApiList() {
  const [cards, setCards] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [deletedCards, setDeletedCards] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [inputValue, setInputValue] = useState('');
  const token = localStorage.getItem("token");
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('https://ghablameh.fiust.ir/api/v1/buffets/', {
          headers: {
            Authorization: `JWT ${token}`,
          },
        });
        setCards(response.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);


  const filteredCards = cards.filter((card) =>
    card.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const onSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const handleInputChange = (event) => {
    setInputValue(event.target.value);
  };


  return (
    <>
      <div className={styles.containment_boof}>
        <Navbarparent />
        <NotificationContainer />
        <div className={styles.itemscenter + " " + "mb-20"}>
          <Boofeh searchTerm={searchTerm} onSearchChange={onSearchChange} />
          {filteredCards.length === 0 ? (
            <div className='flex flex-col  justify-center items-center'>
            <p className='text-lg mb-7'> شما عضو سازمانی نیستید. </p>
            <Link to='/chooseOrg' className='bg-sky-800 hover:bg-sky-900 text-white rounded-lg py-2 px-2'> برای عضویت در سازمان ها کلیک کنید. </Link>
          </div>
          ) : (
            filteredCards.map((card, index) => (
              <Cards
                key={index}
                name={card.name}
                counter_organ={card.counter_organ}
                index={card.id}
              />
            ))
          )}
        </div>
        {/* <ToastContainer /> */}
      </div>
    </>
  );
}

export default DataFromApiList;
