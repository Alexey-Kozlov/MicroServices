export interface IOrder {
    id: number;
    orderDate: Date;
    userId: string;
    description: string;
    productId: number[];
}

export class Order implements IOrder {
    id: number;
    orderDate: Date;
    userId: string;
    description: string;
    productId: number[];

    constructor(id: number = 0, orderDate: Date, userId: string, description: string, productId: number[]) {
        this.id = id;
        this.orderDate = orderDate;
        this.userId = userId;
        this.description = description;
        this.productId = productId;
    }
}
