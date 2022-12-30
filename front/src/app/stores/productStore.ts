import { IProduct } from '../models/iproduct';
import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { IOrder } from '../models/iorder';
import { IProductItems } from '../models/iproductItems';

export default class ProductStore {
    productRegistry = new Map<number, IProduct>();
    productItems = new Map<number, { id: number,productId: number, name: string, quantity: number }>();
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

    public getProduct = async (productId?: string) => {
        this.setIsLoading(true);
        try {
            let data = this.productRegistry.get(Number.parseInt(productId!));
            if (!data) {
                data = await agent.Product.getProductById(productId!) || undefined;
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
            this.getProduct(product.productId.toString()).then((item) => {
                runInAction(() => {
                    this.productItems.set(product.id, { id: product.id, productId: item!.id, name: item!.name, quantity: product.quantity });
                });
            });
        });
    }

    public addUpdateProductItem = (item?: IProductItems) => {
        if (!item) {
            let minIdItem = this.getMinId(this.productItems);
            let minProdIdItem = this.getMinId(this.productRegistry, false);
            this.productItems.set(minIdItem, { id: minIdItem, productId: minProdIdItem, name: '', quantity: 0 });
        } else {
            this.getProduct(item.productId.toString()).then((_item) => {
                runInAction(() => {
                    this.productItems.set(item!.id, { id: item!.id, productId: item.productId, name: _item!.name, quantity: item.quantity });
                });
            });
        }
    };

    public deleteProductItem = (id: number) => {
        this.productItems.delete(id);
    }

    private getMinId = (items: Map<number, any>, nextId: boolean = true) => {
        let minIdItem = Math.min(...[...items.keys()]);
        if (nextId) {
            if (minIdItem > 0) {
                minIdItem = 0;
            }
            minIdItem = minIdItem - 1;
        }
        return minIdItem;
    };

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