import { Tab, Tabs } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import agent from "../api/agent";
import { IIdentity } from "../models/identity";
import { store } from "../stores/store";

interface prop {
    theme: React.CSSProperties
}

export default observer(function LoginTab({ theme }: prop) {
    const {identity,isLoggedIn } = store.identityStore;
    const [_identity, setIdentity] = useState<IIdentity>();
    const url = useLocation();
    useEffect(() => {
        if (isLoggedIn) {
            setIdentity(identity!)
        } else {
            store.identityStore.getIdentity().then((identity) => {
                if (identity) {
                    setIdentity(identity!);
                    if (url.pathname.toLowerCase() === "/unauthorised") {
                        store.commonStore.navigation!('/');
                    }
                } else {
                    if (url.pathname.toLowerCase() != "/unauthorised") {
                        store.commonStore.navigation!('/unauthorised');
                    }
                }
            });
        }
    }, [setIdentity]);

    const handleLogout = () => {
        store.identityStore.logout()
            .then(() => store.commonStore.navigation!('/'))
            .catch(error => alert(error));
    }
    const getActiveTab = () => {
        return isLoggedIn ? 1 : 0;  
    }

    const loginStyle = {
        position: "absolute",
        right:"10px"
    }
    const handleTest = () => {
        agent.Identity.test();
    }

    return (
        <Tabs value={getActiveTab()} sx={loginStyle} TabIndicatorProps={{ sx: { display: "none" } }}>
            {isLoggedIn && <Tab label={_identity && _identity!.displayName}
                onClick={() => handleTest() } sx={theme} />}
            {isLoggedIn && <Tab label="Выйти" onClick={() => handleLogout()} sx={theme} />}                
            {!isLoggedIn && <Tab label="Логин" href={process.env.REACT_APP_IDENTITY! + '/login'} sx={theme} />}
            
        </Tabs>        
    )
})