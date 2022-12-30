export interface IProductItems {
    id: number;
    productId: number;
    quantity: number;
}

export class ProductItems implements IProductItems{
    id: number;
    productId: number;
    quantity: number;

    constructor(id: number, productId: number, quantity: number) {
        this.id = id;
        this.productId = productId;
        this.quantity = quantity;
    }
}

