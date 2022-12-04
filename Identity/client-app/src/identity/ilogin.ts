export interface ILogin {
    login: string;
    password: string;
}

export class Login implements ILogin {
    login: string;
    password: string;

    constructor(login: string, password: string) {
        this.login = login;
        this.password = password;
    }
}