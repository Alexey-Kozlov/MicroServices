import './App.css';
import { Route, Routes } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import ProductMain from '../../features/products/ProductMain';
import ProductForm from '../../features/products/ProductForm';
import Header from '../../features/header/Header';
import CategoryMain from '../../features/category/CategoryMain';
import CategoryForm from '../../features/category/CategoryForm';
import GetToken from './getToken';
import UnAthorized from '../../features/identity/unathorized';
import { ToastContainer } from 'react-toastify';
import "react-toastify/dist/ReactToastify.css";
import OrdersMain from '../../features/orders/ordersMain';
import OrderForm from '../../features/orders/orderForm';

export default observer(function App() {
  return (
      <>
          <ToastContainer position='bottom-right' hideProgressBar />
          <Header />
          <Routes>
              <Route path="/" element={<p>Главная</p>}></Route>
              <Route path="/unathorized" element={<UnAthorized />}></Route>
              <Route path="/products" element={<ProductMain />}></Route>
              <Route path="/product" element={<ProductForm />}></Route>
              <Route path="/product/:id" element={<ProductForm />}></Route>
              <Route path="/category" element={<CategoryMain />}></Route>
              <Route path="/addCategory" element={<CategoryForm />}></Route>
              <Route path="/category/:id" element={<CategoryForm />}></Route>
              <Route path="/token" element={<GetToken />}></Route>
              <Route path="/orders" element={<OrdersMain />}></Route>
              <Route path="/orders" element={<OrderForm />}></Route>
              <Route path="/orders/:id" element={<OrderForm />}></Route>
          </Routes>
    </>
  );
})

