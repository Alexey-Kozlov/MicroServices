import { makeAutoObservable } from 'mobx';
import resourceJsin from '../../resources.json';

export default class CommonStore {
    token: string | null = window.localStorage.getItem(process.env.REACT_APP_TOKEN_NAME!);
    resourcesRegistry = new Map<string, string>();

    constructor() {
        makeAutoObservable(this);
    }

    public getResource = (area: string, name: string) => {
        if (this.resourcesRegistry.size == 0) {
            this.setResources().then(() => {
                return this.resourcesRegistry.get(area + '_' + name);
            })
        } else {
            return this.resourcesRegistry.get(area + '_' + name);
        }
    }

    public setResources = async () => {
        resourceJsin.resources.map(item => (
            this.resourcesRegistry.set(item.area + '_' + item.name.id, item.name.value)
        ));
    }

    public setToken = (token: string) => {
        window.localStorage.setItem(process.env.REACT_APP_TOKEN_NAME!, token!);
        this.token = token;
    }

    public removeToken = () => {
        window.localStorage.removeItem(process.env.REACT_APP_TOKEN_NAME!);
        this.token = null;
    }
}

