import { ICategory } from '../models/icategory';
import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';

export default class CategoryStore{
    categoryRegistry = new Map<number, ICategory>();
    selectedCategory: ICategory | undefined;
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
    public getCategoryList = async () => {
        this.setIsLoading(true);
        try {
            const data = await agent.Category.getCategoryList();
            runInAction(() => {
                data && data.forEach(category => {
                    this.categoryRegistry.set(category.id, category);
                    this.setIsLoading(false);
                });
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
    public getCategoryById = async (id: string) => {
        this.setIsLoading(true);
        try {
            const category = await agent.Category.getCategoryById(id)
            runInAction(() => {
                category && this.categoryRegistry.set(category.id, category);
            });
            this.setIsLoading(false);
            return category;
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
    public addEditCategory = async (category: ICategory) => {
        this.setIsLoading(true);
        try {
            const data = await agent.Category.addEdit(category);
            runInAction(() => {
               data &&  this.categoryRegistry.set(data.id, data);
            });
            this.setIsLoading(false);
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
    public deleteCategory = async (id: number) => {
        this.setIsLoading(true);
        try {
            await agent.Category.delete(id);
            runInAction(() => {
                this.categoryRegistry.delete(id);
                this.setIsLoading(false);
            });
        } catch (error) {
            this.setIsLoading(false);
            return Promise.reject();
        }
    }
}