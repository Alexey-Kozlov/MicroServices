export interface INewAccount {
    login: string;
    password: string;
    password2: string;
    displayName: string;
}

export class NewAccount implements INewAccount {
    login: string;
    password: string;
    password2: string;
    displayName: string;

    constructor(login: string, password: string, password2: string,  displayName: string) {
        this.login = login;
        this.password = password;
        this.password2 = password2;
        this.displayName = displayName;
    }
}