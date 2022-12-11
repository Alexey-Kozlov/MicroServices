import axios, { AxiosError, AxiosResponse } from 'axios';
import { ICategory } from '../models/icategory';
import { IIdentity } from '../models/identity';
import { ILogin } from '../models/ilogin';
import { IProduct } from '../models/iproduct';
import { ResponseResult } from '../models/responseResult';
import { store } from '../stores/store';

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (token) config.headers!.Authorization = `Bearer ${token}`;
    return config;
});

axios.interceptors.response.use(async response => {
    //await sleep(1000);
    return response;
}, (error: AxiosError) => {
    const navigate = store.commonStore.navigation;
    if (error.response) {
        const { data, status, config } = error.response!;
        error.response!.data = getErrorText(data, error);
        console.log("Agent error - " + error.response!.data);        
        switch (status) {
            case 404:
                if (config.method === 'get') {
                    navigate!('/not-found');
                }
                break;
            case 401:
                navigate!('/unathorized');
                break;
        }       
    }
    console.log("Agent error - " + error.stack);
    return Promise.reject(error);
});

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    });
}

const getErrorText = (data: any, error: AxiosError) => {
    let errorText = '';
    let errorData: string[] = [];
    if ((data as any).errors) {
        errorData = (data as any).errors;
    } else {
        errorData[0] = data as string;
    }
    if (errorData) {
        let errorsList = '';
        for (const key in errorData) {
            if (errorData[key]) {
                if ((errorData[key] as any).errorText) {
                    errorsList += errorsList === '' ? (errorData[key] as any).errorText : "," + (errorData[key] as any).errorText;
                } else {
                    errorsList += errorsList === '' ? errorData[key] : "," + errorData[key];
                }
            }
        }
        errorText = errorsList;
    }
    if (!errorText) {
        errorText = error.message;
    }
    return errorText;
}
const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const Product = {
    getProducts: () => axios.get<ResponseResult<IProduct[]>>(process.env.REACT_APP_PRODUCT_API! +
        '/products').then(responseBody),
    getProductById: (id: string) => axios.get<ResponseResult<IProduct>>(process.env.REACT_APP_PRODUCT_API! +
        `/products/${id}`).then(responseBody),
    addEdit: (product: IProduct) => axios.post<ResponseResult<IProduct>>(process.env.REACT_APP_PRODUCT_API! +
        `/products`, { ...product }).then(responseBody),
    delete: (id: number) => axios.delete<ResponseResult<IProduct>>(process.env.REACT_APP_PRODUCT_API! +
        `/products/${id.toString()}`).then(responseBody)

}

const Category = {
    getCategoryList: () => axios.get<ICategory[]>(process.env.REACT_APP_MAIN! +
        '/api/category/GetCategoryList').then(responseBody),
    getCategoryById: (id: string) => axios.get<ICategory>(process.env.REACT_APP_MAIN! +
        `/api/category/${id}`).then(responseBody),
    addEdit: (category: ICategory) => axios.post<ICategory>(process.env.REACT_APP_MAIN! +
        `/api/category`, { ...category }).then(responseBody),
    delete: (id: number) => axios.delete<ICategory>(process.env.REACT_APP_MAIN! +
        `/api/category/${id.toString()}`).then(responseBody)
}

const Identity = {
    login: (login: ILogin) => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY! +
            '/login', { ...login }).then(responseBody),
    identity: (token: string) => {
        const axInstance = axios.create({
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        return axInstance.get<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY! +
            '/CurrentUser').then(responseBody);
    },
    test: () => {
        return axios.get<ResponseResult<object>>(process.env.REACT_APP_MAIN! + '/home/login').then(responseBody)
    },
    register: (user: IIdentity) => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY! +
        '/register', user).then(responseBody),    
    refreshToken: () => axios.post<ResponseResult<IIdentity>>(process.env.REACT_APP_IDENTITY! +
        '/refreshToken', {}).then(responseBody)
}

const agent = {
    Product,
    Category,
    Identity
}

export default agent;
