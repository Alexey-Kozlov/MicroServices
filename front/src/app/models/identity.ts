export interface IIdentity {
    login: string;
    displayName: string;
    token: string;
    isAdmin: boolean;
}

export class Identity implements IIdentity {
    login: string;
    displayName: string;
    token: string;
    isAdmin: boolean;

    constructor(login: string, displayName: string, token: string, isAdmin: boolean) {
        this.login = login;
        this.displayName = displayName;
        this.token = token;
        this.isAdmin = isAdmin;
    }
}