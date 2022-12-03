import { Tab, Tabs } from "@mui/material";
import { useEffect, useState } from "react";
import { IIdentity } from "../models/identity";
import { store, useStore } from "../stores/store";

interface prop {
    theme: React.CSSProperties
}

export default function LoginTab({ theme }: prop) {
    const [identity, setIdentity] = useState<IIdentity>();
    useEffect(() => {
        if (store.identityStore.isLoggedIn) {
            setIdentity(store.identityStore.identity!)
        } else {
            store.identityStore.getIdentity().then((identity) => {
                setIdentity(identity);
            });
        }
    });

    const handleLogout = () => {
        store.identityStore.logout()
            .then(() => store.commonStore.navigation!('/'))
            .catch(error => alert(error));
    }

    const identity_url = '/login';
    const loginStyle = {
        position: "absolute",
        right:"10px"
    }

    return (
        <Tabs value={0} sx={loginStyle} TabIndicatorProps={{ sx: { display: "none" } }}>
            {store.identityStore.isLoggedIn ? (
                <>
                    <Tab label={identity!.displayName} href="/" sx={theme} />
                    <Tab label="Выйти" onClick={() => handleLogout() } sx={theme} />
                </>
                )
                : <Tab label="Логин" href={identity_url} sx={theme} />
                } 
           
            
        </Tabs>
    )
}