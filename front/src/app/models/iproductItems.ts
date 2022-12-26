export interface IProductItems {
    id: number;
    quantity: number;
}

export class ProductItems implements IProductItems{
    id: number;
    quantity: number;

    constructor(id: number, quantity: number) {
        this.id = id;
        this.quantity = quantity;
    }
}

