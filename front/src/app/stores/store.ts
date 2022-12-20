import { createContext, useContext } from "react";
import CategoryStore from "./categoryStore";
import CommonStore from "./commonStore";
import ProductStore from "./productStore";
import IdentityStore from './identityStore';
import OrdersStore from "./ordersStore";
import Paging from "./paging";

interface Store {
    commonStore: CommonStore;
    productStore: ProductStore;
    categoryStore: CategoryStore;
    identityStore: IdentityStore;
    orderStore: OrdersStore;
    paging: Paging;
}

export const store: Store = {
    commonStore: new CommonStore(),
    productStore: new ProductStore(),
    categoryStore: new CategoryStore(),
    identityStore: new IdentityStore(),
    orderStore: new OrdersStore(),
    paging: new Paging()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}