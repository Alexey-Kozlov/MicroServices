import { Tab, Tabs } from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { IIdentity } from "../models/identity";
import { store } from "../stores/store";

interface prop {
    theme: React.CSSProperties
}

export default observer(function LoginTab({ theme }: prop) {
    const {identity,isLoggedIn } = store.identityStore;
    const [_identity, setIdentity] = useState<IIdentity>();
    useEffect(() => {
        if (isLoggedIn) {
            setIdentity(identity!)
        } else {
            store.identityStore.getIdentity().then((identity) => {
                setIdentity(identity!);
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

    const identity_url = process.env.REACT_APP_IDENTITY!+'/login';
    const loginStyle = {
        position: "absolute",
        right:"10px"
    }

    return (
        <Tabs value={getActiveTab()} sx={loginStyle} TabIndicatorProps={{ sx: { display: "none" } }}>
            {isLoggedIn && <Tab label={_identity && _identity!.displayName} href="/" sx={theme} />}
            {isLoggedIn && <Tab label="Выйти" onClick={() => handleLogout()} sx={theme} />}                
            {!isLoggedIn && <Tab label="Логин" href={identity_url} sx={theme} />}
            
        </Tabs>        
    )
})