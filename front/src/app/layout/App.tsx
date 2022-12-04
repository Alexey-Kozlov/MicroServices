import './App.css';
import { Route, Routes, useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { store, useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';
import ProductMain from '../../features/products/ProductMain';
import ProductForm from '../../features/products/ProductForm';
import Header from '../../features/header/Header';
import CategoryMain from '../../features/category/CategoryMain';
import CategoryAdd from '../../features/category/CategoryAdd';
import GetToken from './getToken';

export default observer(function App() {
    const navigation = useNavigate();

    useEffect(() => {
        store.commonStore.setNavigation(navigation);
    });

  return (
      <>
          <Header />
          <Routes>
              <Route path="/" element={<p>Главная</p>}></Route>
              <Route path="/products" element={<ProductMain />}></Route>
              <Route path="/product" element={<ProductForm />}></Route>
              <Route path="/product/:id" element={<ProductForm />}></Route>
              <Route path="/category" element={<CategoryMain />}></Route>
              <Route path="/addCategory" element={<CategoryAdd />}></Route>
              <Route path="/token" element={<GetToken />}></Route>
          </Routes>
    </>
  );
})

