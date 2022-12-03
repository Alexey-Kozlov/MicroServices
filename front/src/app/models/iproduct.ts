export interface IProduct {
    id: number;
    name: string;
    price: number;
    description: string;
    categoryId: number;
    imageId: number;
}

export class Product implements IProduct {
    id: number;
    name: string;
    price: number;
    description: string;
    categoryId: number;
    imageId: number;

    constructor(id: number = 0, name: string, price: number,
        description: string = "", categoryId: number, imageId: number) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.description = description;
        this.categoryId = categoryId;
        this.imageId = imageId;
    }
}
