import axios, { AxiosError, AxiosResponse } from 'axios';
import { IIdentity } from './identityModel';
import { ILogin } from './iloginModel';
import { INewAccount } from './inewAccount';
import { ResponseResult } from './responseResult';

axios.interceptors.response.use(async response => {
    return response;
}, (error: AxiosError) => {
    if (error.response) {
        if (error.response!.data && ((error.response!.data) as any).errors) {
            console.log("Agent error - " + ((error.response!.data) as any).errors[0]);
            return Promise.reject(((error.response!.data) as any).errors[0]);
        }
        console.log("Agent error - " + error.response!.data);
        return Promise.reject(error.response!.data);
    }
    console.log("Agent error - " + error.stack);
    return Promise.reject(error);
});


const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const Identity = {
    login: (login: ILogin) => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY_API! +
            '/login', { ...login }).then(responseBody),
    identity: (token: string) => {
        const axInstance = axios.create({
            headers: {
                Authorization : `Bearer ${token}`
            }
        });
        return axInstance.get<ResponseResult<IIdentity>>('/CurrentUser').then(responseBody);
    },
    register: (user: INewAccount) => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY_API! +
        '/register', { ...user }).then(responseBody),
    refreshToken: () => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY_API! +
        '/refreshToken', {}).then(responseBody)
}

const agent = {
    Identity
}

export default agent;
