import Api from './Api';

export const SearchFriendServices = async (username, token, searchQuery) => {
    try 
    {
        const response = await Api.get(`/User/SearchFirend?search=${searchQuery}`, {
            headers: { 
                "Username": username,
                "Token": token,
            },
        });

        return response;
    } 
    catch (error) 
    {
    }
};