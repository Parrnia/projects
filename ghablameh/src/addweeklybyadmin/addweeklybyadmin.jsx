import React, { useState, useEffect } from 'react';
import style from './addweeklybyadmin.module.css';
import axios from 'axios';
// import Calendar from 'react-calendar';
import Navbarparent from '../components/navbarparent';
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
      console.log('sdxfcgvhbjnklljhgv 47')
      console.log('Data retrieved:', response.data);
      setbuffeh(response.data);
      const ids = response.data.map((buffet) => buffet.id);
      setBuffetId(ids);
      console.log('All buffet IDs:', ids);
    } catch (error) {
      console.error('Error retrieving data:', error);
    }
  };

  const handleMealChange = (e) => {
    const selectedMealId = e.target.value;
    setSelectedMeal(selectedMealId);
    fetchMeals(buffetId, selectedMenuDate);

  };

  const handleFoodChange = (e) => {
    setSelectedFood(e.target.value);
  };

  const handleBuffetChange = (buffetId) => {
    setSelectedBuffetId(buffetId);
    const debouncedFetchData = debounce(() => {
      fetchMeals(buffetId, selectedMenuDate1);
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
  const handleAddFoodItem = async () => {
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
      await fetchFoods(); // Fetch the updated list of foods
    } catch (error) {
      console.error('Error adding new food item:', error);
    }
  };
  const handleNewFoodItemChange = (e) => {
    setNewFoodItem({ ...newFoodItem, [e.target.name]: e.target.value });
  };
  
  
  const addMeals = async () => {
    const token = 'JWT ' + localStorage.getItem('token');
    const buffetId = selectedBuffetId;
  
    const data = {
      name: menuName,
      time: menuTime,
    };
  
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
      // setMenuName('');
      // setMenuTime('');
      console.log('Meals added:', response.data);
    } catch (error) {
      console.error('Error adding meals:', error);
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
  const handleMenuDateChange1 = (date) => {
    const formattedDate = date.toISOString().split('T')[0];

    setSelectedMenuDate1(formattedDate);
    const debouncedFetchData = debounce(() => {
      console.log('sdxfcgvhbjnklljhgv')
      fetchMeals(selectedBuffetId, formattedDate);
      fetchFoods();
      console.log('sdxfcgvhbjnklljhgv 164')
    }, 500);
    debouncedFetchData();
  };

  const fetchMeals = async (buffetId, selectedMenuDate) => {
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.get(`https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${selectedMenuDate}/meals/`, {
        headers: {
          Authorization: token,
        },
      });

      console.log('Meals retrieved:', response.data);
      setMeals(response.data);
    } catch (error) {
      console.error('Error retrieving meals:', error);
    }
  };

  const fetchFoods = async () => {
    console.log('sdxfcgvhbjnklljhgv')
    const token = 'JWT ' + localStorage.getItem('token');

    try {
      const response = await axios.get('https://ghablameh.fiust.ir/api/v1/foods/', {
        headers: {
          Authorization: token,
        },
      });

      // console.log('Foods retrieved:', response.data);
      // console.log('sdxfcgvhbjnklljhgv')
      setFoods(response.data);
      console.log('foods: ',foods)
    } catch (error) {
      console.error('Error retrieving foods:', error);
    }
  };

  const addMealItem = async () => {
    const token = 'JWT ' + localStorage.getItem('token');
    const buffetId = selectedBuffetId;
    const menuDate = selectedMenuDate1;
    const mealId = selectedMeal;

    const data = {
      food: selectedFood,
      price: price, // Use the price state variable
      number_in_stock: numberInStock, // Use the numberInStock state variable
    };
   
    console.log(`https://ghablameh.fiust.ir/api/v1/buffets/${buffetId}/menus/${menuDate}/meals/${mealId}/items/`);
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
      
      console.log('Meal item added:', response.data);
    } catch (error) {
      console.error('Error adding meal item:', error);
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
    <div>
    <Navbarparent />
    <div className={style['weekly-menu-card']}>
      <div className={style['buffet-selection']}>
        <label htmlFor="buffet-select">بوفه مورد نظر را انتخاب کنید</label>
        <select className={style.selectt} id="buffet-select" onChange={(e) => handleBuffetChange(e.target.value)}>
          <option value="">انتخاب بوفه</option>
          {buffeh.map((item) => (
            <option key={item.id} value={item.id}>
              {item.name} - {item.organization}
            </option>
          ))}
        </select>
      </div>

      <div className={style['menu-items-container']}>
        {menuItems.map((item, index) => (
          <div key={index} className={style['menu-item-card']}>
            <input type="text" className={style['menu-item-input']} value={item.name} disabled />
            <select className={style['menu-item-select']} value={item.type} disabled>
              {/* Options */}
            </select>
            <button className={style.week} onClick={() => handleRemoveMenuItem(index)}>
              Remove
            </button>
          </div>
        ))}
      </div>

      <div className={style['new-food-item-container']}>
        <h3 className={style.h33}>غذای مورد نظر خود را اضافه کنید</h3>
        <input
          type="text"
          name="name"
          className={style.weekinput}
          placeholder="اسم غذا"
          value={newFoodItem.name}
          onChange={handleNewFoodItemChange}
        />
        <input
          type="text"
          name="description"
          className={style.weekinput}
          placeholder="توضیح غذا"
          value={newFoodItem.description}
          onChange={handleNewFoodItemChange}
        />
        <button className={style.week} onClick={handleAddFoodItem}>
          اضافه کردن غذا
        </button>
      </div>

      <h3 className={style.h33}>وعده مورد نظر خود را اضافه کنید</h3>
      <input
        type="text"
        className={style.weekinput}
        placeholder="وعده غذایی"
        value={menuName}
        onChange={(e) => setMenuName(e.target.value)}
      />
      <input
        type="text"
        className={style.weekinput}
        placeholder="ساعت"
        value={menuTime}
        onChange={(e) => setMenuTime(e.target.value)}
      />
      <div className={style['menu-date-picker']}>
        <label htmlFor="menu-date-input"></label>
        <input
          type="date"
          id="menu-date-input"
          value={selectedMenuDate}
          onChange={(e) => handleMenuDateChange(new Date(e.target.value))}
        />
      </div>
      <button className={style.week} onClick={addMeals}>
        اضافه کردن وعده
      </button>

      <h3 className={style.h33}>منوی هفتگی مورد نظر خود را اضافه کنید</h3>
      <input
        type="text"
        className={style.weekinput}
        placeholder="هزینه"
        value={price}
        onChange={(e) => setPrice(e.target.value)}
      />
      <input
        type="number"
        className={style.weekinput}
        placeholder="تعداد"
        value={numberInStock}
        onChange={(e) => setNumberInStock(e.target.value)}
      />
      <div className={style['menu-date-picker']}>
        <label htmlFor="menu-date-input"></label>
        <input
          type="date"
          id="menu-date-input"
          value={selectedMenuDate1}
          onChange={(e) => handleMenuDateChange1(new Date(e.target.value))}
        />
      </div>
      <select className={style.selectt} onChange={handleMealChange}>
        <option value="">انتخاب وعده</option>
        {meals.map((meal) => (
          <option key={meal.id} value={meal.id}>
            {meal.name}
          </option>
        ))}
      </select>

      <select className={style.selectt} onChange={handleFoodChange}>
        <option value="">انتخاب غذا</option>
        {foods.results && foods.results.map((food) => (
          <option key={food.id} value={food.id}>
            {food.name}
          </option>
        ))}
      </select>

      <button className={style.week} onClick={addMealItem}>
    اضافه کردن منو
      </button>
    </div>
  </div>
  
  );
};

export default AddWeeklyMenu; 