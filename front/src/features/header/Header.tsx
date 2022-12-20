import { AppBar, Tab, Tabs, ThemeProvider, Toolbar } from "@mui/material";
import HeaderTheme from "../header/headerTheme";
import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import LoginTab from "./LoginTab";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function Header() {


    const url = useLocation();

    useEffect(() => {
        let currentMenu = {
            tabNumber: 0
        }
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
                case checkUrl("/products"):
                    currentMenu.tabNumber = 1;
                    break;
                case checkUrl("/category"):
                case checkUrl("/addCategory"):
                    currentMenu.tabNumber = 2;
                    break;
                case checkUrl("/orders"):
                    currentMenu.tabNumber = 3;
                    break;
            }
            return currentMenu;
        }
        setTabState(getActiveTab(url.pathname).tabNumber);
    }, [url.pathname]);

    const [tabState, setTabState] = useState(0);

    const { identityStore: { isLoggedIn } } = useStore();
    return (
        <ThemeProvider theme={HeaderTheme}>
            <AppBar position="fixed" >
                <Toolbar disableGutters>
                    {isLoggedIn &&
                        <Tabs value={tabState}>
                            <Tab label="Главная" sx={HeaderTheme.typography.tab} component={Link} to={"/"} />
                            <Tab label="Продукты" sx={HeaderTheme.typography.tab} component={Link} to={"/products" } />
                            <Tab label="Категории" sx={HeaderTheme.typography.tab} component={Link} to={"/category"} />
                            <Tab label="Заказы" sx={HeaderTheme.typography.tab} component={Link} to={"/orders"} />
                        </Tabs>
                    }
                    <LoginTab theme={HeaderTheme.typography.tab } />
                </Toolbar>
            </AppBar>
            <div style={{ height: "80px" }}></div>
        </ThemeProvider>

    )
})