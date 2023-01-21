export interface ILogin {
    login: string;
    password: string;
}

export class Login implements ILogin {
    constructor(public login: string, public password: string) {
        this.login = login;
        this.password = password;
    }
}