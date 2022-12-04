import { makeAutoObservable, runInAction } from "mobx";
import { NavigateFunction, useNavigate } from "react-router-dom";
import { IIdentity } from "../models/identity";
import { store } from "./store";
import agent from "../api/agent";


export default class IdentityStore {
    identity: IIdentity | null = null;
    refreshTokenTimeout: any;
    navigation: NavigateFunction | undefined;

    constructor() {
        makeAutoObservable(this);
    }

    setNavigation = (navigation: NavigateFunction) => {
        this.navigation = navigation;
    }

    get isLoggedIn() {
        return !!this.identity;
    }

    get isAdmin() {
        return this.identity?.isAdmin;
    }

    logout = async () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.identity = null;
    }

    getIdentity = async () => {
        //обращаемся к сервису аутентификации для проверки токена
        const token = window.localStorage.getItem('jwt');
        if (token) {            
            //получаем полное Identity
            const identity = await agent.Identity.identity(token);
            runInAction(() => store.identityStore.identity = identity.result);               
            return identity.result;           
        } 
        return null;
    }

    //register = async (formValue: IUserFormValues) => {
    //    try {
    //        const user = await agent.Account.register(formValue);
    //        store.commonStore.setToken(user.token);
    //        this.startRefreshTokenTimer(user);
    //        runInAction(() => this.user = user);
    //        this.navigation!("/activities");
    //        store.modalStore.closeModal();
    //    } catch (error) {
    //        console.log(error);
    //        throw error;
    //    }
    //}

    //regreshToken = async () => {
    //    this.stopRefreshTokenTimer();
    //    try {
    //        const user = await agent.Account.refreshToken();
    //        runInAction(() => this.user = user);
    //        store.commonStore.setToken(user.token);
    //        this.startRefreshTokenTimer(user);
    //    } catch (error) {
    //        console.log(error);
    //        return Promise.reject(error);
    //    }
    //}

    //private startRefreshTokenTimer(user: IUser) {
    //    const token = JSON.parse(atob(user.token.split('.')[1]));
    //    const expires = new Date(token.exp * 1000);
    //    const timeOut = expires.getTime() - Date.now() - (30 * 1000); //начать обновлять токен за 30 секунд до его истечения
    //    this.refreshTokenTimeout = setTimeout(this.regreshToken, timeOut);
    //}

    //private stopRefreshTokenTimer() {
    //    clearTimeout(this.refreshTokenTimeout);
    //}
}