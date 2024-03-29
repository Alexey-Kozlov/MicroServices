import { makeAutoObservable, runInAction } from "mobx";
import { IIdentity } from "../models/identity";
import { store } from "./store";
import agent from "../api/agent";


export default class IdentityStore {
    identity: IIdentity | null = null;
    refreshTokenTimeout: any;

    constructor() {
        makeAutoObservable(this);
    }

    clearIdentity = () => {
        store.commonStore.removeToken();
        this.identity = null;
    }

    get isLoggedIn() {
        return !!this.identity;
    }

    get isAdmin() {
        return this.identity?.isAdmin;
    }

    logout = async () => {
        this.clearIdentity();
    }

    getIdentity = async () => {
        //обращаемся к сервису аутентификации для проверки токена
        if (this.identity) {
            return this.identity;
        } else {
            //получаем Identity
            const identity = await agent.Identity.getIdentity();
            if (identity && identity.isSuccess) {
                runInAction(() => this.identity = identity.result);
                this.startRefreshTokenTimer(identity.result);
                return identity.result;
            } else {
                return null;
            }
        }
    }

    refreshToken = async () => {    
        try {
            const identity = await agent.Identity.refreshToken();
            runInAction(() => this.identity = identity!.result!);
            store.commonStore.setToken(identity!.result!.token);
            this.startRefreshTokenTimer(this.identity!);
        } catch (error) {
            console.log(error);
            return Promise.reject(error);
        }
    }

    public startRefreshTokenTimer(identity: IIdentity) {
        if (this.refreshTokenTimeout) {
            this.stopRefreshTokenTimer();
        }
        const token = JSON.parse(atob(identity.token.split('.')[1]));
        const expires = new Date(token.exp * 1000);
        let timeOut = 0;
        //если параметр REACT_APP_BREAK_INACTIVITY == true - означает прервать сессию после истечения срока действия токена и послать на логин
        //если параметр REACT_APP_BREAK_INACTIVITY != true - означает за 30 сек.дл истечения срока действия токена послать запрос на его обновление
        if (process.env.REACT_APP_BREAK_INACTIVITY === 'true') {
            timeOut = expires.getTime() - Date.now() + (5 * 1000); //начать проверять токен через 5 секунд после его истечения
        } else {
            timeOut = expires.getTime() - Date.now() - (30 * 1000); //начать обновлять токен за 30 секунд до его истечения
        }
        this.refreshTokenTimeout = setTimeout(this.refreshToken, timeOut);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }
}