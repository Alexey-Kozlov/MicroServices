import { Tab, Tabs } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import agent from "../../app/api/agent";
import { IIdentity } from "../../app/models/identity";
import { store } from "../../app/stores/store";

interface prop {
    theme: React.CSSProperties
}

export default observer(function LoginTab({ theme }: prop) {
    const { isLoggedIn, identity, refreshToken } = store.identityStore;
    const [_identity, setIdentity] = useState<IIdentity>();
    const url = useLocation();
    const navigate = useNavigate();
    useEffect(() => {
        //если перенаправление на присваивание токена - отменяем логику
        if (url.pathname.toLowerCase() === "/token") {
            return;
        }
        if (isLoggedIn) {
            agent.Identity.refreshToken()!.then((token) => {
                setIdentity(token!.result);
            });                        
            if (url.pathname.toLowerCase() === "/unathorized") {
                navigate('/');
            }
        } else {
            if (window.localStorage.getItem(process.env.REACT_APP_TOKEN_NAME!) && url.pathname.toLowerCase() !== "/unathorized") {
                refreshToken();            
            }
        }
    }, []);



    const handleLogout = () => {
        store.identityStore.logout()
            .then(() => navigate("/"))
            .catch(error => alert(error));
    }
    const getActiveTab = () => {
        return isLoggedIn ? 1 : 0;  
    }

    const loginStyle = {
        position: "absolute",
        right:"10px"
    }

    return (
        <Tabs value={getActiveTab()} sx={loginStyle} TabIndicatorProps={{ sx: { display: "none" } }}>
            {isLoggedIn && <Tab label="Выйти" onClick={() => handleLogout()} sx={theme} />}                
            {!isLoggedIn && <Tab label="Логин" href={process.env.REACT_APP_LOGIN! + '/login'} sx={theme} />}
            
        </Tabs>        
    )


})