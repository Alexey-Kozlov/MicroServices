import { IProductItems } from "./iproductItems";

export interface IOrder {
    id: number;
    orderDate: Date;
    userId: string;
    description: string;
    products: IProductItems[];
}

export class Order implements IOrder {
    id: number;
    orderDate: Date;
    userId: string;
    description: string;
    products: IProductItems[];

    constructor(id: number = 0, orderDate: Date, userId: string, description: string, products: IProductItems[]) {
        this.id = id;
        this.orderDate = orderDate;
        this.userId = userId;
        this.description = description;
        this.products = products;
    }
}
