import { useEffect } from "react"
import { useNavigate, useSearchParams } from "react-router-dom";
import { store } from "../stores/store";

export default function GetToken() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    useEffect(() => {
        const token = searchParams.get('token');
        const retUrl = searchParams.get('ReturnUrl');

        //получили токен
        if (token) {
            store.commonStore.setToken(token);
            store.identityStore.getIdentity().then(() => {
                let url = "/";
                if (retUrl) {
                    url = url + retUrl;
                }
                navigate(url);
            })
        }
    }, []);
    return (
        <p></p>
    )
}