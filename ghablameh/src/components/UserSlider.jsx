// import React, { useState } from "react";
// import styles from "../styles/UserSlider.module.css";
// const Slider = () => {
//   function hasDecimal(number) {
//     return !Number.isInteger(number);
//   }
//   const data = [
//     {
//       name: "بوفه تست",
//       usercount: 25,
//       rate: 5,
//       image: "D:/Projects/Univercity/Tahlil/Pull-apart-garlic-rolls-with-baked-cheese-4132baa.jpg",
//     },
//     {
//       name: "1بوفه تست",
//       usercount: 25,
//       rate: 3,
//       image: "D:/Projects/Univercity/Tahlil/Pull-apart-garlic-rolls-with-baked-cheese-4132baa.jpg",
//     },
//     {
//       name: "2بوفه تست",
//       usercount: 25,
//       rate: 4.5,
//       image: "D:/Projects/Univercity/Tahlil/Pull-apart-garlic-rolls-with-baked-cheese-4132baa.jpg",
//     },
//   ];
//   const star = (
//     <svg
//       className="w-[16px] h-[16px] text-gray-800 dark:text-white"
//       aria-hidden="true"
//       xmlns="http://www.w3.org/2000/svg"
//       width="24"
//       height="24"
//       fill="currentColor"
//       viewBox="0 0 24 24"
//       style={{ display: "inline" }}
//     >
//       <path d="M13.849 4.22c-.684-1.626-3.014-1.626-3.698 0L8.397 8.387l-4.552.361c-1.775.14-2.495 2.331-1.142 3.477l3.468 2.937-1.06 4.392c-.413 1.713 1.472 3.067 2.992 2.149L12 19.35l3.897 2.354c1.52.918 3.405-.436 2.992-2.15l-1.06-4.39 3.468-2.938c1.353-1.146.633-3.336-1.142-3.477l-4.552-.36-1.754-4.17Z" />
//     </svg>
//   );
//   const halfStar = (
//     <svg
//       className="w-[16px] h-[16px] text-gray-800 dark:text-white"
//       aria-hidden="true"
//       xmlns="http://www.w3.org/2000/svg"
//       width="24"
//       height="24"
//       fill="currentColor"
//       viewBox="0 0 24 24"
//       style={{ display: "inline" }}
//     >
//       <path
//         fillRule="evenodd"
//         d="M13 4.024v-.005c0-.053.002-.353-.217-.632a1.013 1.013 0 0 0-1.176-.315c-.192.076-.315.193-.35.225-.052.05-.094.1-.122.134a4.358 4.358 0 0 0-.31.457c-.207.343-.484.84-.773 1.375a168.719 168.719 0 0 0-1.606 3.074h-.002l-4.599.367c-1.775.14-2.495 2.339-1.143 3.488L6.17 15.14l-1.06 4.406c-.412 1.72 1.472 3.078 2.992 2.157l3.94-2.388c.592-.359.958-.996.958-1.692v-13.6Zm-2.002 0v.025-.025Z"
//         clipRule="evenodd"
//       />
//     </svg>
//   );
//   const [currentIndex, setCurrentIndex] = useState(0);
//   const handlePrev = () => {
//     setCurrentIndex((prevIndex) =>
//       prevIndex === 0 ? data.length - 1 : prevIndex - 1
//     );
//   };

//   const handleNext = () => {
//     setCurrentIndex((prevIndex) =>
//       prevIndex === data.length - 1 ? 0 : prevIndex + 1
//     );
//   };
//   const Stars = (index) => {
//     let madeStars = [];
//     if(hasDecimal(data[index].rate)) {
//       madeStars.push(halfStar);
//     }
//     for(let i = 0;i<Math.floor(data[index].rate);i++) {
//       madeStars.push(star);
//     }

//     return madeStars;
//   }
//   return (
//     <div className="border border-sky-800 rounded mx-5 p-2 grid grid-cols-11">
//       <button onClick={handlePrev} className="flex justify-center items-center">
//         <svg
//           className="w-6 h-6 text-gray-800 dark:text-white"
//           aria-hidden="true"
//           xmlns="http://www.w3.org/2000/svg"
//           width="24"
//           height="24"
//           fill="none"
//           viewBox="0 0 24 24"
//         >
//           <path
//             stroke="currentColor"
//             strokeLinecap="round"
//             strokeLinejoin="round"
//             strokeWidth="2"
//             d="m9 5 7 7-7 7"
//           />
//         </svg>
//       </button>
//       <div className="col-span-9 text-center mx-auto ">
//         {console.log(data[currentIndex].image)}
//         <img src={data[currentIndex].image} className="rounded-2" />
//         <br />
//         <h3>{data[currentIndex].name}</h3>
//         <div className="grid grid-cols-2">
//           <div>
//             {Stars(currentIndex)}
//           </div>
//           <div className="text-left">{data[currentIndex].usercount} کاربر</div>
//         </div>
//         <button className="w-100 border-sky-800 rounded bg-sky-600 p-2 text-white hover:bg-sky-800">
//           مشاهده برنامه غذایی بوفه
//         </button>
//       </div>
//       <button onClick={handleNext} className="flex justify-center items-center">
//         <svg
//           className="w-6 h-6 text-gray-800 dark:text-white"
//           aria-hidden="true"
//           xmlns="http://www.w3.org/2000/svg"
//           width="24"
//           height="24"
//           fill="none"
//           viewBox="0 0 24 24"
//         >
//           <path
//             stroke="currentColor"
//             strokeLinecap="round"
//             strokeLinejoin="round"
//             strokeWidth="2"
//             d="m15 19-7-7 7-7"
//           />
//         </svg>
//       </button>
//     </div>
//   );
// };

// export default Slider;
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";
import Organizations from "../APIs/Organizations";

const responsive = {
  desktop: {
    breakpoint: { max: 3000, min: 1024 },
    items: 3,
    slidesToSlide: 3, // optional, default to 1.
  },
  tablet: {
    breakpoint: { max: 1024, min: 464 },
    items: 2,
    slidesToSlide: 2, // optional, default to 1.
  },
  mobile: {
    breakpoint: { max: 464, min: 0 },
    items: 1,
    slidesToSlide: 1, // optional, default to 1.
  },
};


// const data = [
//   {
//     name: "بوفه تست",
//     usercount: 25,
//     rate: 5,
//     image:
//     "src\\assets\\images.jpeg",
//   },
//   {
//     name: "1بوفه تست",
//     usercount: 25,
//     rate: 3,
//     image:
//     "src\\assets\\images.jpeg",
//   },
//   {
//     name: "2بوفه تست",
//     usercount: 25,
//     rate: 4.5,
//     image:
//     "src\\assets\\images.jpeg",
//   },
//   {
//     name: "2بوفه تست",
//     usercount: 25,
//     rate: 4.5,
//     image:
//     "src\\assets\\images.jpeg",
//   },
//   {
//     name: "2بوفه تست",
//     usercount: 25,
//     rate: 4.5,
//     image:
//     "src\\assets\\images.jpeg",
//   },
// ];
const star = (
  <svg
    className="w-[16px] h-[16px] text-gray-800 dark:text-white"
    aria-hidden="true"
    xmlns="http://www.w3.org/2000/svg"
    width="24"
    height="24"
    fill="currentColor"
    viewBox="0 0 24 24"
    style={{ display: "inline" }}
  >
    <path d="M13.849 4.22c-.684-1.626-3.014-1.626-3.698 0L8.397 8.387l-4.552.361c-1.775.14-2.495 2.331-1.142 3.477l3.468 2.937-1.06 4.392c-.413 1.713 1.472 3.067 2.992 2.149L12 19.35l3.897 2.354c1.52.918 3.405-.436 2.992-2.15l-1.06-4.39 3.468-2.938c1.353-1.146.633-3.336-1.142-3.477l-4.552-.36-1.754-4.17Z" />
  </svg>
);
const halfStar = (
  <svg
    className="w-[16px] h-[16px] text-gray-800 dark:text-white"
    aria-hidden="true"
    xmlns="http://www.w3.org/2000/svg"
    width="24"
    height="24"
    fill="currentColor"
    viewBox="0 0 24 24"
    style={{ display: "inline" }}
  >
    <path
      fillRule="evenodd"
      d="M13 4.024v-.005c0-.053.002-.353-.217-.632a1.013 1.013 0 0 0-1.176-.315c-.192.076-.315.193-.35.225-.052.05-.094.1-.122.134a4.358 4.358 0 0 0-.31.457c-.207.343-.484.84-.773 1.375a168.719 168.719 0 0 0-1.606 3.074h-.002l-4.599.367c-1.775.14-2.495 2.339-1.143 3.488L6.17 15.14l-1.06 4.406c-.412 1.72 1.472 3.078 2.992 2.157l3.94-2.388c.592-.359.958-.996.958-1.692v-13.6Zm-2.002 0v.025-.025Z"
      clipRule="evenodd"
    />
  </svg>
);
const Stars = (index) => {
  let madeStars = [];
  for (let i = 0; i < Math.floor(data[index].rate); i++) {
    madeStars.push(star);
  }
  if (hasDecimal(data[index].rate)) {
    madeStars.push(halfStar);
  }
  return madeStars;
};
function hasDecimal(number) {
  return !Number.isInteger(number);
}
const UserSlider = async () => {
  const data = await Organizations.Top5Buffets();
  return (
    <Carousel
      swipeable={false}
      draggable={false}
      showDots={true}
      responsive={responsive}
      ssr={true} // means to render carousel on server-side.
      infinite={true}
      //autoPlay={this.props.deviceType !== "mobile" ? true : false}
      autoPlaySpeed={1000}
      keyBoardControl={true}
      customTransition="all .5"
      transitionDuration={500}
      containerClass="carousel-container"
      removeArrowOnDeviceType={["tablet", "mobile"]}
      //deviceType={this.props.deviceType}
      dotListClass="custom-dot-list-style"
      itemClass="carousel-item-padding-40-px"
    >
      {data.map((item) => ( // "src\assets\images.jpeg"
        <div
          
          className="border border-sky-800 rounded p-2 mx-2  text-center"
        >
          <img
            src={item.image}
            className="w-full"
            style={{ maxHeight: "150px" }}
          />
          <h2 style={{ textAlign: "center", fontSize: "24px" }}>{item.name}</h2>
          <div className="grid grid-cols-2">
            {/* <div className="m-1 text-center">{item.}</div> */}
            <div className="m-1 text-center">{Stars(data.indexOf(item))}</div>
            <div className="m-1 text-center">{item.usercount} کاربر</div>
          </div>
          
          <button className="w-100 border-sky-800 rounded bg-sky-600 p-2 text-white hover:bg-sky-800 my-5">
            مشاهده برنامه غذایی بوفه
          </button>
        </div>
      ))}
    </Carousel>
  );
};

export default UserSlider;
