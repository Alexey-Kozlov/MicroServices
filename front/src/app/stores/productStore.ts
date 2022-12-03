import { IProduct } from '../models/iproduct';
import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';

export default class ProductStore {
    productRegistry = new Map<number, IProduct>();
    selectedProduct: IProduct | undefined;
    isLoading: boolean = true;
    isSubmitted: boolean = false;

    constructor() {
        makeAutoObservable(this);
    }

    setIsLoading = (val: boolean) => {
        this.isLoading = val;
    }

    setIsSubmitted = (val: boolean) => {
        this.isSubmitted = val;
    }

    setSelectedProduct = (val: IProduct | undefined) => {
        this.selectedProduct = val;
    }

    public getProducts = async () => {
        this.setIsLoading(true);
        try {
            const data = await agent.Product.getProducts();
            runInAction(() => {
                data.result.forEach(product => {
                    this.productRegistry.set(product.id, product);
                    this.setIsLoading(false);
                });
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public getProduct = async (id: string) => {
        this.setIsLoading(true);
        try {
            const data = await agent.Product.getProductById(id);
            runInAction(() => {
                this.productRegistry.set(data.result.id, data.result);
                this.selectedProduct = data.result;
                this.setIsLoading(false);
            });
            return data.result;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public addEditProduct = async (product: IProduct) => {
        this.setIsSubmitted(true);
        try {
            const data = await agent.Product.addEdit(product);
            runInAction(() => {
                this.productRegistry.set(data.result.id, data.result);
                this.setIsSubmitted(false);
            });
           
        } catch (error) {
            this.setIsSubmitted(false);
            return Promise.reject();
        }
    }

    public deleteProduct = async (id: number) => {
        this.setIsLoading(true);
        try {
            await agent.Product.delete(id);
            runInAction(() => {
                this.productRegistry.delete(id);
                this.setIsLoading(false);
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
}