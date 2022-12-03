import { ICategory } from '../models/icategory';
import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';

export default class CategoryStore{
    categoryRegistry = new Map<number, ICategory>();
    constructor() {
        makeAutoObservable(this);
    }
    public getCategoryList = async () => {
        const data = await agent.Category.getCategoryList();
        runInAction(() => {
            data.result.forEach(category => {
                this.categoryRegistry.set(category.id, category);
            });
        });
    }
    public addCategory = async (category: ICategory) => {
        const data = await agent.Category.create(category);
        runInAction(() => {
            this.categoryRegistry.set(data.result.id, data.result);
        });
    }
}