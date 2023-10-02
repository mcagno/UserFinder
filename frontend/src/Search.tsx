import {APIService, User} from "./APIService.ts";
import {FormEvent, useRef, useState} from "react";

export function Search() {
    const [searchString, setSearchString] = useState("");
    const [searchResult, setSearchResult] = useState<User[]>([]);
    const lastNameRef = useRef<HTMLInputElement>(null);
    const firstNameRef = useRef<HTMLInputElement>(null);
    
    const submitSearchForm = async (event: FormEvent) => {
        event.preventDefault();
        console.log("New search")
        await APIService.search(searchString).then(users => setSearchResult(users));
    }

    /*useEffect(() => {
        fetch('https://jsonplaceholder.typicode.com/posts?_limit=10')
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setPosts(data);
            })
            .catch((err) => {
                console.log(err.message);
            });
    }, []);*/

    
    const handleInputChange = (event: React.FormEvent<HTMLInputElement>) => {
        setSearchString(event.currentTarget.value);
    };

    const submitInsertForm = async (event: FormEvent) => {
        event.preventDefault()
        if (firstNameRef.current != null && lastNameRef.current != null)
            await APIService.insert(firstNameRef.current.value, lastNameRef.current?.value);
        
    };
    
    return (
    <>
        <form onSubmit={submitSearchForm}>
            <input type="text" value={searchString} onChange={handleInputChange}/>
            <button type="submit">Search</button>
        </form>
        <div>
            {searchResult.length == 0 ?
                "No result" :
                searchResult.map((user) =>
                    <div key={user.id}>
                        <div >
                            {user.firstName}
                        </div>
                        <div >
                            {user.lastName}
                        </div>
                    </div>
                )}
        </div>
        <form onSubmit={submitInsertForm}>
            First name <input type="text" ref={firstNameRef} />
            Last name <input type="text" ref={lastNameRef} />
            <button type="submit">Insert</button>
        </form>
    </>
    )
}