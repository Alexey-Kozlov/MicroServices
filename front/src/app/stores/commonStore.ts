import { makeAutoObservable, reaction } from 'mobx';

export default class CommonStore {
    token: string | null = window.localStorage.getItem(process.env.REACT_APP_TOKEN_NAME!);

    constructor() {
        makeAutoObservable(this);
        reaction(() => this.token, token => {
            //if (token) {
            //    this.setToken(token);
            //} else {
            //    window.localStorage.removeItem(process.env.REACT_APP_TOKEN_NAME!);
            //}
            if (!token) {
                window.localStorage.removeItem(process.env.REACT_APP_TOKEN_NAME!);
            }
        });
    }

    setToken = (token: string | null) => {
        window.localStorage.setItem(process.env.REACT_APP_TOKEN_NAME!, token!);
        this.token = token;
    }
}

