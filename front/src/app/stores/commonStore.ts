import { NavigateFunction } from 'react-router-dom';
import { makeAutoObservable, reaction } from 'mobx';

export default class CommonStore {
    navigation: NavigateFunction | undefined;
    token: string | null = window.localStorage.getItem('jwt');

    constructor() {
        makeAutoObservable(this);
        reaction(() => this.token, token => {
            if (token) {
                window.localStorage.setItem('jwt', token!);
            } else {
                window.localStorage.removeItem('jwt');
            }
        });
    }

    setNavigation = (navigation: NavigateFunction) => {
        this.navigation = navigation;
    }

    setToken = (token: string | null) => {
        this.token = token;
    }
}

