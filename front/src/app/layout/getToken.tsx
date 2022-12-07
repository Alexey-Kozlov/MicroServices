import { useEffect } from "react"
import { useSearchParams } from "react-router-dom";
import { store } from "../stores/store";

export default function GetToken() {
    const [searchParams] = useSearchParams();
    useEffect(() => {
        const token = searchParams.get('token');
        const retUrl = searchParams.get('ReturnUrl');
        //получили токен
        if (token) {
            store.commonStore.setToken(token);
            let url = process.env.REACT_APP_FRONT!;
            if (retUrl) {
                url = url + retUrl;
            }
            window.location.href = url;
        }
    }, [searchParams]);
    return (
        <p></p>
    )
}