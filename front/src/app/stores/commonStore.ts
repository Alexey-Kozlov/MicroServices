import { makeAutoObservable } from 'mobx';

export default class CommonStore {
    token: string | null = window.localStorage.getItem(process.env.REACT_APP_TOKEN_NAME!);

    constructor() {
        makeAutoObservable(this);
    }

    setToken = (token: string | null) => {
        window.localStorage.setItem(process.env.REACT_APP_TOKEN_NAME!, token!);
        this.token = token;
    }

    removeToken = () => {
        window.localStorage.removeItem(process.env.REACT_APP_TOKEN_NAME!);
        this.token = null;
    }
}

