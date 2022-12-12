import axios, { AxiosError, AxiosResponse } from 'axios';
import { ICategory } from '../models/icategory';
import { IIdentity } from '../models/identity';
import { ILogin } from '../models/ilogin';
import { IProduct } from '../models/iproduct';
import { ResponseResult } from '../models/responseResult';
import { store } from '../stores/store';
import { toast } from 'react-toastify';

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
            case 500:
                let mes = getErrorText(data, error);
                toast.error(getCustomexceptionMessage(mes));
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
const getCustomexceptionMessage = (data: string) => {
    const begIndex = data.indexOf('was thrown.');
    const endIndex = data.indexOf('HEADERS');
    return data.substring(begIndex + 11, endIndex);
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
const responseBody = <T>(response: AxiosResponse<T>) => {
    if (response) {
        return response.data;
    }
    return null;
}

const Product = {
    getProducts: () => axios.get<IProduct[]>(process.env.REACT_APP_MAIN! +
        '/api/product/getproductlist').then(responseBody),
    getProductById: (id: string) => axios.get<IProduct>(process.env.REACT_APP_MAIN! +
        `/api/product/${id}`).then(responseBody),
    addEdit: (product: IProduct) => axios.post<IProduct>(process.env.REACT_APP_MAIN! +
        `/api/product`, { ...product }).then(responseBody),
    delete: (id: number) => axios.delete<IProduct>(process.env.REACT_APP_MAIN! +
        `/api/product/${id.toString()}`).then(responseBody)

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
        axInstance.interceptors.response.use(async response => {
            return response;
        }, (error: AxiosError) => {
            if (error.response) {
                const { status } = error.response!;
                const navigate = store.commonStore.navigation;
                if (status == 401) {
                    navigate!('/unathorized');
                }
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
