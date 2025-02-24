// import React, { useState, useEffect } from "react";
// import Organizations from "../APIs/Organizations";

// const BueffetsDropdown = async () => {
//     let data = await Organizations.GetOrganizationBuffets();

//     return (
//         <div className="grid grid-cols-3 w-full">
//             <div></div>
//             <div className="content-center w-full">
//                 <select className="rounded w-full">
//                     {data.length > 0 ? (
//                         data.map((item) => (
//                             <option key={item.id} value={item.id}>
//                                 {item.name}
//                             </option>
//                         ))
//                     ) : (
//                         <option>Loading...</option>
//                     )}
//                 </select>
//             </div>
//             <div></div>
//         </div>
//     );
// };

// export default BueffetsDropdown;
import React, { useState, useEffect } from "react";
import Organizations from "../APIs/Organizations";

const BuffetsDropdown = () => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                let result = await Organizations.GetOrganizationBuffets();
                setData(result);
            } catch (error) {
                console.error("Error fetching data:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    return (
        <div className="grid grid-cols-3 w-full">
            <div></div>
            <div className="
content-center w-full">
                <select className="rounded w-full">
                    {loading ? (
                        <option>Loading...</option>
                    ) : (
                        data.map((item) => (
                            <option key={item.id} value={item.id}>
                                {item.name}
                            </option>
                        ))
                    )}
                </select>
            </div>
            <div></div>
        </div>
    );
};

export default BuffetsDropdown;
