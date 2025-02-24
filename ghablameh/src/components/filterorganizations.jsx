import { useState, useEffect, useRef } from "react";
import { AiOutlineReload, AiOutlineSearch } from "react-icons/ai";
import filterIcon from "/filter.png";

const FilterOrganizations = () => {
    //product list
    const productList = [
        { name: "blue pant", rate: 2 },
        { name: "black pant", rate: 3 },
        { name: "blue shirt", rate: 2 },
        { name: "black shoes", rate: 1 },
        { name: "brown shoes", rate: 1 },
        { name: "white pant", rate: 4 },
        { name: "white shoes", rate: 5 },
        { name: "red shirt", rate: 5 },
        { name: "gray pant", rate: 5 },
        { name: "white shirt", rate: 3 },
        { name: "golden shoes", rate: 3 },
        { name: "dark pant", rate: 2 },
        { name: "pink shirt", rate: 1 },
        { name: "yellow pant", rate: 4 }
    ];

    const [searchTerm, setSearchTerm] = useState("");
    const [ratingFilter, setRatingFilter] = useState(null);
    const [isOpen, setIsOpen] = useState(false);
    const [selectedRating, setSelectedRating] = useState(null);

    //rate
    const handleRatingFilter = () => {
        setIsOpen(true);
    };

    const applyRatingFilter = () => {
        setRatingFilter(selectedRating);
        setIsOpen(false);
    };

    const handleStarClick = (rating) => {
        setSelectedRating(rating);
    };

    //reset
    const resetFilters = () => {
        setSearchTerm("");
        setRatingFilter(null);
        setSelectedRating(null);
    };


    //dropdown
    const dropdownRef = useRef(null);
    useEffect(() => {
        document.addEventListener('click', handleClickOutside);
        return () => {
            document.removeEventListener('click', handleClickOutside);
        };
    }, []);

    const handleClickOutside = (e) => {
        if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
            setIsOpen(false);
        }
    };

    return (
        <div>
            {/* <Navbar /> */}
            <div className="max-w-md mx-auto mt-4 mb-4 p-4 rounded-lg" style={{ backgroundColor: "rgb(38, 87, 124)", height: "20rem", overflowY: "scroll", border: "1px solid rgb(38, 87, 124)" }}>
                <div className="flex items-center mb-4">
                    
                    <div className="relative w-full flex items-center">
                        <input type="text"  placeholder="Search products..." value={searchTerm} 
                            onChange={(event) => {setSearchTerm(event.target.value);}}
                            className="w-full p-2 pl-10 rounded-md items-start"
                            style={{ border: "1px solid rgb(38, 87, 124)" }}
                        />
                        <AiOutlineSearch className="text-gray-500 m-4" style={{ fontSize: "1.4rem", marginLeft: "-47px" }} />
                    </div>

                    <div className="ml-2">
                        <div className="ml-2">
                            <div onClick={handleRatingFilter} style={{ cursor: "pointer" }}>
                                <img src={filterIcon} alt="Filter Icon" style={{ width: 40, height: 40, filter: "invert(100%)" }} />
                            </div>
                        </div>
                    </div>

                    <div className="ml-2">
                        <AiOutlineReload size={24} onClick={resetFilters} style={{ cursor: "pointer", filter: "invert(100%)" }} />
                    </div>
                </div>

                {productList.filter((product) => {
                    if ((searchTerm === "" || product.name.toLowerCase().includes(searchTerm.toLowerCase())) && (ratingFilter === null || product.rate === ratingFilter)) {
                        return product;
                    }
                }).map((product, key) => {
                    return (
                        <div key={key} className="bg-white p-2 mb-2 rounded-md flex items-start" style={{ border: "1px solid rgb(38, 87, 124)" }}>
                            <p>{product.name}</p>
                        </div>
                    );
                })}
                
                {/* Dropdown */}
              <div className={` ${isOpen ? '' : 'hidden'} rounded-lg shadow`} style={{ backgroundColor: 'white', margin: '0.3vw' }}>
                <ul className='py-1 text-sm text-white'>
                  <li><a className='block px-4 py-2'>userpanel</a></li>
                  <li><a  className='block px-4 py-2'>Logout</a></li>
                </ul>
              </div>

            </div>
          </div>


    );
};

export default FilterOrganizations;

// {isOpen && (
//     <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center bg-gray-800 bg-opacity-50">
//         <div className="bg-white p-4 max-w-sm rounded-md" style={{ position: "relative" }}>
//             <button onClick={() => setIsOpen(false)} className="absolute top-0 right-0 p-2 cursor-pointer" style={{ zIndex: 1 }}>
//                 X
//             </button>
//             <div>
//                 {[1, 2, 3, 4, 5].map((rating) => (
//                     <span key={rating} onClick={() => handleStarClick(rating)} style={{ cursor: "pointer" }}>
//                         {selectedRating >= rating ? "⭐️" : "☆"}
//                     </span>
//                 ))}
//             </div>
//             <button onClick={applyRatingFilter} className="bg-blue-500 text-white p-2 rounded-md">Apply Filter</button>
//         </div>
//     </div>
// )}
