import axios from 'axios';
import AuthManager from "../APIs/AuthManager";
import { useState, useEffect } from "react";
import Select from 'react-select';
import defaultPhoto from '/team.png'
import styles from '../styles/organizationlist.module.css'
import { Link } from 'react-router-dom';

function OrganizationList() {
    const [orgData, setOrgData] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [rateFilter, setRateFilter] = useState('');
    const [flag, setFlag] = useState(null);

    //style for filter
    const customStyles = {
        control: (provided, state) => ({
            ...provided,
            borderColor: state.isFocused ? 'rgb(38, 87, 124)' : 'rgb(38, 87, 124)', // Changes the border color when the select is focused or not.
            borderRadius: '8px',
            backgroundColor: 'rgba(255, 255, 255, 0.6)',
            boxShadow: state.isFocused ? '0 0 0 1px #26577c' : 'none',
        }),
    };

    //fetch flag
    useEffect(() => {
        const checkUserType = async () => {
            const userType = await AuthManager.orguser();
            setFlag(userType);
        };

        checkUserType();
    }, []);

    //fetch organizations
    const fetchOrganizations = async () => {
        try {
            const token = AuthManager.getToken();
            const response = await axios.get('https://ghablameh.fiust.ir/api/v1/organizations/', {
                headers: { Authorization: "JWT " + token }
            });
            setOrgData(response.data);
        } catch (error) {
            console.error('Error fetching organizations: ', error);

        }
    };

    // fetch oragnizations every 5 seconds
    useEffect(() => {
        // Fetch immediately for the first time
        if (AuthManager.isLoggedIn()) fetchOrganizations();
        // const interval = setInterval(() => {
            // if (AuthManager.isLoggedIn()) fetchOrganizations();
        // });
         // (fetch every 5 seconds)

        // Clear the interval when the component is unmounted
        // return () => clearInterval(interval);
    },[]);
    //search
    const handleSearchChange = (e) => {
        setSearchTerm(e.target.value);
    };

    //select/filter
    const rateOptions = [
        { value: '', label: 'امتیاز' },
        { value: '1', label: '1' },
        { value: '2', label: '2' },
        { value: '3', label: '3' },
        { value: '4', label: '4' },
        { value: '5', label: '5' }
    ];

    const handleRateFilterChange = (selectedOption) => {
        setRateFilter(parseInt(selectedOption.value));
    };

    const filteredOrgData = orgData
        .filter(org => org.name.toLowerCase().includes(searchTerm.toLowerCase()))
        .filter(org => rateFilter ? org.average_rate >= rateFilter && org.average_rate < (rateFilter + 1) : true);

    return (
        <div className="container mx-auto p-4" style={{ minHeight: '30vh' }}>
            {AuthManager.isLoggedIn() && orgData && (
                <>
                    <div className={`flex flex-row gap-4 mb-4 items-center ${styles.wrapper}`}>
                        {/* search */}
                        <input
                            type="text"
                            placeholder="جست و جو سازمان ها ... "
                            value={searchTerm}
                            onChange={handleSearchChange}
                            className={`p-2 border rounded-lg w-full md:w-1/3`}
                            style={{ border: '1px solid rgb(38, 87, 124)', width: '32.7%' }}
                        />

                        {/* filter */}
                        <div className='' style={{ width: '12vw' }}>
                            <Select
                                placeholder="فیلتر بر اساس امتیاز"
                                value={rateOptions.find(option => option.value === rateFilter)}
                                onChange={handleRateFilterChange}
                                options={rateOptions}
                                // md:w-1/7
                                title="فیلتر بر اساس امتیاز"
                                isSearchable={false}
                                // className={`flex flex-row gap-4 mb-4 items-center ${styles.customelect}`}
                                styles={customStyles}
                            />
                        </div>
                    </div>
                </>
            )}

            {/* orgs list */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                {filteredOrgData.map((org, index) => (
                    <div key={index} className="bg-white bg-opacity-50 shadow-md rounded-lg overflow-hidden" style={{ border: '1px solid rgb(38, 87, 124)' }}>
                        {org.image_url ?(
                            <div className="overflow-hidden h-40" style={{ padding: '10px' }}>
                                <img src={'https://ghablameh.fiust.ir' + org.image_url} className="w-full h-full rounded-lg" style={{ objectFit: 'contain' }} />
                            </div>
                        ) : (
                            <div style={{
                                height: "160px",
                                backgroundImage: "repeating-conic-gradient(#26577c  0% 25%, #ffffff 0% 50%)",
                                backgroundPosition: "0 0, 32px 32px",
                                backgroundSize: "50px 50px",
                                opacity:'0.5'
                             }}
                                    >

                                    </div>
                        
                        )}
                        <div className="p-3 flex flex-col justify-start" style={{ borderTop: '1px solid rgb(38, 87, 124)' }}>
                            <h3 className="">{org.name}</h3>
                            <div className="flex justify-between items-center text-sm">
                                <div className="flex-1 text-xs">
                                    {org.average_rate ? (
                                        <span>امتیاز: {org.average_rate}</span>
                                    ) : (
                                        <span>امتیاز: بدون نظر</span>
                                    )}
                                    {/* ({org.number_of_rates} نظر) */}
                                </div>
                                <div>
                                    {flag === 2 ? (
                                        // 1d3e57
                                        <button className='p-2 mt-3 rounded text-white text-xs' style={{ background: 'rgba(249, 115, 22,0.9)' }}><Link to='/chooseOrg'> درخواست عضویت </Link></button>
                                    ) : null}
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default OrganizationList;