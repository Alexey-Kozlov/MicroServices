import { useEffect } from "react"
import { useSearchParams } from "react-router-dom";
import { store } from "../stores/store";

export default function GetToken() {
    const [searchParams] = useSearchParams();
    useEffect(() => {
        const token = searchParams.get('token');
        //получили токен
        if (token) {
            store.commonStore.setToken(token);
            window.location.href = process.env.REACT_APP_FRONT!;
        }
    }, [searchParams]);
    return (
        <p></p>
    )
}