import configData from '../config/config.json'; 




export type User = {
    id: number,
    firstName: string,
    lastName: string
}
export const APIService = {
    
    search : async (searchString: string ) => {
        const fullUrl = configData.userFinderApi.url + "?searchString=" + searchString;
        const response = await fetch(fullUrl);
        const users: User[] = await response.json();
        console.log(users)
        return users;
    },
    
    insert: async (firstName: string | null, lastName: string | null) => {
        const fullUrl = configData.userFinderApi.url;
        let user = {
            firstName: firstName,
            lastName: lastName
        };
        console.log(user);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(user)
        };
        return await fetch(fullUrl, requestOptions);
    }
}