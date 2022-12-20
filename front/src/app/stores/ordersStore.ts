import { makeAutoObservable, reaction, runInAction } from 'mobx';
import agent from '../api/agent';
import { IOrder } from '../models/iorder';
import PagingParams from '../models/pagingParams';
import { store } from './store';

export default class OrdersStore {
    ordersRegistry = new Map<number, IOrder>();
    selectedOrder: IOrder | undefined;
    isLoading: boolean = true;
    isSubmitted: boolean = false;
    predicate = new Map().set('all', true);

    constructor() {
        makeAutoObservable(this);
        reaction(
            () => this.predicate.keys(),
            () => {
                store.paging.pagingParams = new PagingParams();
                this.ordersRegistry.clear();
                this.getOrders();
            }
        )
    }

    setPredicate = (predicate: string, value: string | Date) => {
        this.predicate.delete('all');
        this.predicate.set('all', true);
    }

    get pageParams() {
        const params = store.paging.pageParams;
        this.predicate.forEach((value, key) => {
            params.append(key, value);            
        });
        return params;
    }

    setIsLoading = (val: boolean) => {
        this.isLoading = val;
    }

    setIsSubmitted = (val: boolean) => {
        this.isSubmitted = val;
    }

    setSelectedOrder = (val: IOrder | undefined) => {
        this.selectedOrder = val;
    }

    public getOrders = async () => {
        this.setIsLoading(true);
        try {
            const data = await agent.Orders.getOrdersList(this.pageParams);
            runInAction(() => {
                data && data.result.value.forEach(order => {
                    this.ordersRegistry.set(order.id, order);
                });
                store.paging.setPagination(data!.result!.pagination);

            });
            this.setIsLoading(false);
            return data!.result.value;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public getOrder = async (id: string) => {
        this.setIsLoading(true);
        try {
            const data = await agent.Orders.getOrderById(id);
            runInAction(() => {
                if (data) {
                    this.ordersRegistry.set(data.id, data);
                    this.selectedOrder = data;
                }
                this.setIsLoading(false);
            });
            return data && data;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public addEditOrder = async (order: IOrder) => {
        this.setIsLoading(true);
        try {
            const data = await agent.Orders.addEdit(order);
            runInAction(() => {
                data && this.ordersRegistry.set(data.id, data);
                this.setIsLoading(false);
            });
           
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public deleteOrder = async (id: number) => {
        this.setIsLoading(true);
        try {
            await agent.Orders.delete(id);
            runInAction(() => {
                this.ordersRegistry.delete(id);
                this.setIsLoading(false);
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
}