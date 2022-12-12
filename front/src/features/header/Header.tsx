import { AppBar, Tab, Tabs, ThemeProvider, Toolbar } from "@mui/material";
import HeaderTheme from "../header/headerTheme";
import { useState } from "react";
import { useLocation } from "react-router-dom";
import LoginTab from "./LoginTab";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { ToastContainer } from "react-toastify";

export default observer(function Header() {

    let currentMenu = {
        tabNumber: 0
    }
    const url = useLocation();
    const checkUrl = (url_template: string) => {
        if (url.pathname.toLowerCase().includes(url_template.toLowerCase()))
            return url.pathname.toLowerCase();
    }

    const getActiveTab = (url: string) => {
        currentMenu.tabNumber = 0;
        switch (url.toLowerCase()) {
            case "/":
                currentMenu.tabNumber = 0;
                break;
            case checkUrl("/product"):
                currentMenu.tabNumber = 1;
                break;
            case checkUrl("/category"):
            case checkUrl("/addCategory"):
                currentMenu.tabNumber = 2;
                break;
        }
        return currentMenu;
    }

    const _tabState = getActiveTab(url.pathname);
    const [tabState, setTabState] = useState(_tabState.tabNumber);
    const { identityStore: { isLoggedIn } } = useStore();
    return (
        <ThemeProvider theme={HeaderTheme}>
            <AppBar position="fixed" >
                <Toolbar disableGutters>
                    {isLoggedIn &&
                        <Tabs value={tabState}>
                            <Tab label="Главная" sx={HeaderTheme.typography.tab} href="/" />
                            <Tab label="Продукты" sx={HeaderTheme.typography.tab} href="/products" />
                            <Tab label="Категории" sx={HeaderTheme.typography.tab} href="/category" />
                        </Tabs>
                    }
                    <LoginTab theme={HeaderTheme.typography.tab } />
                </Toolbar>
            </AppBar>
            <div style={{ height: "80px" }}></div>
        </ThemeProvider>

    )
})