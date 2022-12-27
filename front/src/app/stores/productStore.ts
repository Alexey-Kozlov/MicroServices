import { IProduct } from '../models/iproduct';
import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { IOrder } from '../models/iorder';

export default class ProductStore {
    productRegistry = new Map<number, IProduct>();
    productItems = new Map<number, { id: number, name: string, quantity: number }>();
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
                data && data.forEach(product => {
                    this.productRegistry.set(product.id, product);
                });
                this.setIsLoading(false);
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public getProduct = async (id: string) => {
        if (!id) {
            return undefined;
        }
        this.setIsLoading(true);
        try {
            let data = this.productRegistry.get(Number.parseInt(id));
            if (!data) {
                data = await agent.Product.getProductById(id) || undefined;
                runInAction(() => {
                    if (data) {
                        this.productRegistry.set(data.id, data);
                        this.selectedProduct = data;
                    }
                });
            }
            this.setIsLoading(false);
            return data;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public getProductItems = (order: IOrder) => {
        this.productItems.clear();
        order.products.forEach((product) => {
            this.getProduct(product.id.toString()).then((item) => {
                runInAction(() => {
                    this.productItems.set(item!.id, { id: item!.id, name: item!.name, quantity: product.quantity });
                });
            });
        });
    }

    public addEditProduct = async (product: IProduct) => {
        this.setIsLoading(true);
        try {
            const data = await agent.Product.addEdit(product);
            runInAction(() => {
                data && this.productRegistry.set(data.id, data);
                this.setIsLoading(false);
            });
           
        } catch (error) {
            this.setIsLoading(false);
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