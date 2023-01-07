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
    predicate = new Map<string,string>();

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

    setPredicate = (predicate: string, value: string) => {
        switch (predicate) {
            case 'sort':
                if (this.predicate.get('sort')) {
                    this.predicate.delete('sort');
                }
                break;
        }
       
        this.predicate.set(predicate, value);
    }

    get pageParams() {
        const params = store.paging.pageParams;
        this.predicate.forEach((value : string, key: string) => {
            switch (key) {
                case 'sort':
                    const sortData = value.split(',');
                    params.append('sortField', sortData[0]);
                    params.append('sortDirection', sortData[1]);      
                    break;
            }               
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
        this.ordersRegistry.clear();
        try {
            const data = await agent.Orders.getOrdersList(this.pageParams);
            runInAction(() => {
                data && data.Items.forEach(order => {
                    this.ordersRegistry.set(order.id, order);
                });
                store.paging.setPagination(data!.pagination);
            });
            this.setIsLoading(false);
            return data!.Items;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }

    public getOrder = async (id: string) => {
        this.setIsLoading(true);
        try {
            if (!id) {
                return undefined;
            }
            let data = this.ordersRegistry.get(Number.parseInt(id));
            if (!data) {
                data = await agent.Orders.getOrderById(id) || undefined;
                runInAction(() => {
                    if (data) {
                        this.ordersRegistry.set(data.id, data);
                    }
                });
            } 
            this.selectedOrder = data;            
            this.setIsLoading(false);
            return data;
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
                this.ordersRegistry.delete(0);
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