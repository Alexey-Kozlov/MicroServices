import { makeAutoObservable, runInAction } from "mobx";
import { NavigateFunction, useNavigate } from "react-router-dom";
import agent from "../api/agent";
import { IIdentity } from "../models/identity";
import { ILogin } from "../models/ilogin";
//import { IUserFormValues } from "../models/iuserFormValues";
import { store } from "./store";



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

    login = async (formValue: ILogin) => {
        try {
            const identity = await agent.Identity.login(formValue);
            store.commonStore.setToken(identity.result.token);
            //this.startRefreshTokenTimer(user);
            runInAction(() => this.identity = identity.result);
            //store.modalStore.closeModal();
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    logout = async () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.identity = null;
    }

    getIdentity = async () => {
        try {
            const identity = await agent.Identity.identity();
            store.commonStore.setToken(identity.result.token);
            runInAction(() => this.identity = identity.result);
            return identity.result;
            //this.startRefreshTokenTimer(user);
        } catch (error) {
            console.log(error);
            return Promise.reject(error);
        }
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