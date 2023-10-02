import './App.css'
import {useNavigate} from "react-router-dom";

function Home() {
    const navigate = useNavigate();

    return (
        <>
            <h1>Welcome to my new page</h1>
            <button onClick={() => navigate('search')}>Click me to search</button>
        </>
    )
}
export default Home