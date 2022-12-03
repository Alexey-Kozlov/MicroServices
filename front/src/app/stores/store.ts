import { createContext, useContext } from "react";
import CategoryStore from "./categoryStore";
import CommonStore from "./commonStore";
import ProductStore from "./productStore";
import IdentityStore from './identityStore';

interface Store {
    commonStore: CommonStore;
    productStore: ProductStore;
    categoryStore: CategoryStore;
    identityStore: IdentityStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    productStore: new ProductStore(),
    categoryStore: new CategoryStore(),
    identityStore: new IdentityStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}