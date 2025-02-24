import React, { useState, useEffect } from 'react';
import style from '../styles/addweeklybyadmin.module.css';
import axios from 'axios';
// import Calendar from 'react-calendar';
import Navbarparent from '../components/navbarparent';
import Select from 'react-select';

import { ToastContainer, toast } from 'react-toastify';
import { NotificationContainer, NotificationManager } from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import '../styles/customNotifications.css';
const AddWeeklyMenu = () => {
  const [buffeh, setbuffeh] = useState([]);
  const [buffetId, setBuffetId] = useState('');
  const [menuItems, setMenuItems] = useState([]);
  const [newMenuItem, setNewMenuItem] = useState('');
  const [newMenuItemType, setNewMenuItemType] = useState('');
  const [selectedBuffetId, setSelectedBuffetId] = useState('');
  const [newFoodItem, setNewFoodItem] = useState({ name: '', description: '' });
  const [menuName, setMenuName] = useState('');
  const [menuTime, setMenuTime] = useState('');
  const [meals, setMeals] = useState([]);
  const [selectedMenuDate, setSelectedMenuDate] = useState('');
  const [selectedMenuDate1, setSelectedMenuDate1] = useState('');
  const [foods, setFoods] = useState([]);
  const [selectedMeal, setSelectedMeal] = useState('');
  const [selectedFood, setSelectedFood] = useState('');
  const [price, setPrice] = useState('');
  const [numberInStock, setNumberInStock] = useState('');
  const [selectedMealId, setShowSelectedMea] = useState('');
  const [showSelectedMeal, setShowSelectedMeal] = useState(false);

  useEffect(() => {
    const fetchDataInParallel = async () => {
      await Promise.all([
        fetchDataFromURL(),
        fetchMeals(selectedBuffetId, selectedMenuDate1),
        fetchFoods(),
      ]);
    };
    fetchDataInParallel();
  }, [selectedBuffetId, selectedMenuDate1, meals]);

  const fetchDataFromURL = async () => {
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.get('https://ghablameh.fiust.ir/api/v1/buffets/', {
        headers: {
          Authorization: token,
        },
      });

      // console.log('Data retrieved:', response.data);
      setbuffeh(response.data);
      const ids = response.data.map((buffet) => buffet.id);
      setBuffetId(ids);
      // console.log('All buffet IDs:', ids);
    } catch (error) {
      console.error('Error retrieving data:', error);
    }
  };

  const handleMealChange = (option) => {
    const selectedMealId = option.value;
    setSelectedMeal(selectedMealId);
    fetchMeals(buffetId, selectedMenuDate);

  };

  const handleFoodChange = (option) => {
    setSelectedFood(option.value);
  };

  const handleBuffetChange = (selectedOption) => {
    setSelectedBuffetId(selectedOption.value);
    const debouncedFetchData = debounce(() => {
      fetchMeals(selectedOption.value, selectedMenuDate1);
      fetchFoods();
    }, 500);
    debouncedFetchData();
  };

  const handleAddMenuItem = () => {
    if (newMenuItem && newMenuItemType) {
      setMenuItems([...menuItems, { name: newMenuItem, type: newMenuItemType }]);
      setNewMenuItem('');
      setNewMenuItemType('');
    }
  };

  const handleRemoveMenuItem = (index) => {
    const updatedMenuItems = [...menuItems];
    updatedMenuItems.splice(index, 1);
    setMenuItems(updatedMenuItems);
  };

  const handleNewFoodItemChange = (e) => {
    setNewFoodItem({ ...newFoodItem, [e.target.name]: e.target.value });
  };

  const addFoodItem = async () => {
    if (!newFoodItem.name) {
      // infoToast();
      createNotification('info')();
      // createNotification('buffet')();
      return;
    }
    // console.log(newFoodItem)
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.post(
        'https://ghablameh.fiust.ir/api/v1/foods/',
        newFoodItem,
        {
          headers: {
            Authorization: token,
            'Content-Type': 'application/json',
          },
        }
      );

      // console.log('New food item added:', response.data);
      setNewFoodItem({ name: '', description: '' });

      // successToast();
      createNotification('succes')();
      await fetchFoods(); // Fetch the updated list of foods
    } catch (error) {
      // console.error('Error adding new food item:', error.message);
      
      if (error.message== "Network Error")
      createNotification('fail',error.message)();

      if (error.response.data.name) {
        // console.log("error.response.data.name: ", error.response.data.name)
        // failToast(error.response.data.name);
        createNotification('fail', error.response.data.name)();
        return;
      }

      //

    }
  };

  const addMeals = async () => {
    const token = 'JWT ' + localStorage.getItem('token');
    if (!selectedBuffetId) {
      // buffetToast();
      createNotification('buffet')();
      return;
    }

    const buffetId = selectedBuffetId;

    const data = {
      name: menuName,
      time: menuTime,
    };

    if (!data.name || !data.time) {
      // infoToast();
      createNotification('info')();
      return;
    }

    try {
      const response = await axios.post(
        `https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${selectedMenuDate}/meals/`,
        data,
        {
          headers: {
            Authorization: token,
            'Content-Type': 'application/json',
          },
        }
      );
      const newMeal = response.data;
      setMeals([...meals, newMeal]); // Update the meals state with the new meal
      // console.log('Meals added:', response.data);
      // successToast();
      createNotification('succes')();
    } catch (error) {
      console.error('Error adding meals:', error);
      if (error.response.data.time) {
        // failToast(error.response.data.time);
        createNotification('fail', error.response.data.time)();
        return;
      }
      if (error.response.data.name) {
        // failToast(error.response.data.name);
        createNotification('fail', error.response.data.name)();
      }

    }
  };

  const addMealItem = async () => {
    if (!selectedBuffetId) {
      // buffetToast();
      createNotification('buffet')();
      return;
    }

    const token = 'JWT ' + localStorage.getItem('token');
    const buffetId = selectedBuffetId;
    const menuDate = selectedMenuDate1;
    const mealId = selectedMeal;

    const data = {
      food: selectedFood,
      price: price, // Use the price state variable
      number_in_stock: numberInStock, // Use the numberInStock state variable
    };
    if (!data.food || !data.price || !data.number_in_stock) {
      // infoToast();
      createNotification('info')();
      return;
    }

    // console.log(`https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${menuDate}/meals/${mealId}/items/`);
    try {
      const response = await axios.post(
        `https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${menuDate}/meals/${mealId}/items/`,
        data,
        {
          headers: {
            Authorization: token,
            'Content-Type': 'application/json',
          },
        }
      );

      // console.log('Meal item added:', response.data);
      // successToast();
      createNotification('succes')();
    } catch (error) {
      console.error('Error adding meal item:', error);
      if (error.response.data.food) {
        // failToast(error.response.data.food);
        createNotification('fail', error.response.data.food)();
        return;
      }
      if (error.response.data.price) {
        // failToast(error.response.data.price);
        createNotification('fail', error.response.data.price)();
        return;
      }
      if (error.response.data.number_in_stock) {
        // failToast(error.response.data.number_in_stock);
        createNotification('fail', error.number_in_stock)();
      }
    }
  };

  const handleMenuDateChange = (date) => {
    const formattedDate = date.toISOString().split('T')[0];
    setSelectedMenuDate(formattedDate);

    const debouncedFetchData = debounce(() => {
      fetchMeals(selectedBuffetId, formattedDate);
      fetchFoods();
    }, 500);
    debouncedFetchData();
  };

  //Toast
  const createNotification = (type,error) => {
    return () => {
      switch (type) {
        case 'succes':
          NotificationManager.success(`با موفقیت اضافه شد `, '', 2000);
          break;
        case 'info':
          NotificationManager.warning(`اطلاعات لازم را وارد کنید `, '', 2000);
          break;
        case 'buffet':
          NotificationManager.warning(' ابتدا بوفه خود را انتخاب کنید  ', '', 2000);
          break;
        case 'fail':
          NotificationManager.error( `${error}`,'مشکلی پیش آمده', 2000);
          break;

        default:
          break;
      }
    };
  };

  const successToast = () => {
    toast.info(
      <div className="flex flex-col items-center">
        <div className="text-center mb-4">{` با موفقیت اضافه شد `}</div>
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

  const failToast = (error) => {
    toast.info(
      <div className="flex flex-col items-center">
        <div className="text-center mb-4">{` ${error} `}</div>
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

  const infoToast = () => {
    toast.info(
      <div className="flex flex-col items-center">
        <div className="text-center mb-4">{` اطلاعات لازم را وارد کنید `}</div>
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

  const buffetToast = () => {
    toast.info(
      <div className="flex flex-col items-center">
        <div className="text-center mb-4">{` ابتدا بوفه خود را انتخاب کنید `}</div>
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

  const handleMenuDateChange1 = (date) => {
    const formattedDate = date.toISOString().split('T')[0];

    setSelectedMenuDate1(formattedDate);
    const debouncedFetchData = debounce(() => {
      fetchMeals(selectedBuffetId, formattedDate);
      fetchFoods();
    }, 500);
    debouncedFetchData();
  };

  const buffetoptions = buffeh.map((item) => ({
    value: item.id,
    label: `${item.name}`,
  }));

  const fetchMeals = async (buffetId, selectedMenuDate) => {
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.get(`https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${selectedMenuDate}/meals/`, {
        headers: {
          Authorization: token,
        },
      });

      // console.log('Meals retrieved:', response.data);
      setMeals(response.data);
    } catch (error) {
      console.error('Error retrieving meals:', error);
    }
  };

  const fetchFoods = async () => {
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.get('https://ghablameh.fiust.ir/api/v1/foods/', {
        headers: {
          Authorization: token,
        },
      });

      // console.log('Foods retrieved:', response.data);
      setFoods(response.data);
      // console.log(response.data)
    } catch (error) {
      console.error('Error retrieving foods:', error);
    }
  };

  const debounce = (func, delay) => {
    let timeoutId;
    return (...args) => {
      clearTimeout(timeoutId);
      timeoutId = setTimeout(() => {
        func.apply(this, args);
      }, delay);
    };
  };



  return (
    <div className='w-full flex flex-col justify-center items-center' >
      <Navbarparent />
      <NotificationContainer />
      {/* buffet */}
      <div className={style['buffet-selection']}>
        <label htmlFor="buffet-select">بوفه مورد نظر را انتخاب کنید.</label>

        <Select className={style.selectt}
          options={buffetoptions}
          onChange={handleBuffetChange}
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
        />
      </div>

      {/* cards */}
      <div className={style['parentContainer']}>

        {/* add food */}
        <div className={style['weekly-menu-card']}>
          <div className={`${style['new-food-item-container']}`}>
            <h3 className={style.h33}> اضافه کردن غذا </h3>
            <hr style={{ border: '0.5px solid #bdbdbd', width: '15rem', marginTop: '0.5rem', marginBottom: '0.5rem' }} />

            {/* <div className='my-7'></div> */}
            <input
              type="text"
              name="name"
              className={style.weekinput}
              placeholder="اسم غذا"
              value={newFoodItem.name}
              onChange={handleNewFoodItemChange}
            />
            {/* <div className='my-1'></div> */}
            <input
              type="text"
              name="description"
              className={style.weekinput}
              placeholder="توضیح غذا"
              value={newFoodItem.description}
              onChange={handleNewFoodItemChange}
            />
            <div className='my-3'></div>

            {/* <div className='my-10'></div>
            <div className='my-5'></div> */}
            <button className={style.week} onClick={addFoodItem}>
              اضافه کردن غذا
            </button>

          </div>
        </div>

        {/* add meal */}
        <div className={style['weekly-menu-card']}>
          <div className={`${style['new-food-item-container']}`}>
            <h3 className={style.h33}> اضافه کردن وعده غذایی </h3>
            <hr style={{ border: '0.5px solid #bdbdbd', width: '15rem', marginTop: '0.5rem', marginBottom: '0.5rem' }} />
            {/* <div className='my-6'></div> */}
            <input
              type="text"
              className={style.weekinput}
              placeholder="نام وعده غذایی"
              value={menuName}
              onChange={(e) => setMenuName(e.target.value)}
            />

            <input
              type="text"
              className={style.weekinput}
              placeholder="ساعت توزیع"
              value={menuTime}
              onChange={(e) => setMenuTime(e.target.value)}
            />
            <div className={style['menu-date-picker']}>
              <label htmlFor="menu-date-input"></label>
              <input
                type="date"
                id="menu-date-input"
                value={selectedMenuDate}
                className='h-10'
                onChange={(e) => handleMenuDateChange(new Date(e.target.value))}
              />
            </div>
            {/* <div className='my-1'></div> */}
            {/* <div className='my-5'></div> */}
            <button className={style.week} onClick={addMeals}>
              اضافه کردن وعده
            </button>
          </div>
        </div>

        {/* add meal food */}
        <div className={style['weekly-menu-card']}>
          <div className={style['new-food-item-container']}>
            <h3 className={style.h33}> اضافه کردن غذا در منو هفتگی </h3>
            <hr style={{ border: '0.5px solid #bdbdbd', width: '15rem', marginTop: '0.5rem', marginBottom: '0.5rem' }} />
            <div className='my-2'></div>


           
            <Select
      className={style.selectt}
      onChange={handleFoodChange}
      options={
        foods.length > 0
          ? foods.map((food) => ({
              value: food.id,
              label: food.name,
            }))
          : []
      }
      placeholder="انتخاب غذا"
      theme={(theme) => ({
        ...theme,
        colors: {
          ...theme.colors,
          text: 'de6016',
          primary: 'rgb(38, 87, 124)',
          primary25: 'rgba(38, 87, 124,0.4)',
        },
      })}
    />
            <Select
              className={style.selectt}
              onChange={handleMealChange}
              options={meals.map((meal) => ({
                value: meal.id,
                label: meal.name,
              }))}
              placeholder={selectedMenuDate1 ? "انتخاب وعده" : "برای انتخاب وعده ابتدا تاریخ را انتخاب کنید."}
              isDisabled={!selectedMenuDate1}
              theme={(theme) => ({
                ...theme,
                colors: {
                  ...theme.colors,
                  text: 'de6016',
                  primary: 'rgb(38, 87, 124)',
                  primary25: 'rgba(38, 87, 124,0.4)',
                },
                // borderRadius:'8px'
              })}
            />
            <div className='flex flex-row w-60 items-center justify-between'>
              {/* <div className='w-32'> */}
              <input
                type="text"
                className={`${style.weekinput} ml-3`}
                placeholder="هزینه"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
              />
              {/* </div> */}
              {/* <div className=''> */}
              <input
                type="number"
                className={`${style.weekinput}`}
                placeholder="تعداد"
                value={numberInStock}
                onChange={(e) => setNumberInStock(e.target.value)}
              />
            </div>

            <div className={`${style['menu-date-picker']}`}>
              <label htmlFor="menu-date-input"></label>
              <input
                type="date"
                id="menu-date-input"
                className='h-10'
                value={selectedMenuDate1}
                onChange={(e) => handleMenuDateChange1(new Date(e.target.value))}
              />
            </div>

            <button className={style.week} onClick={addMealItem}>
              اضافه کردن منو
            </button>
          </div>

        </div>
      </div>

      <ToastContainer />
    </div>
  );
};

export default AddWeeklyMenu; 