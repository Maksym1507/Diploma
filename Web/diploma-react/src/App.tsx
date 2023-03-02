import React, { FC, } from 'react';
import './App.css';
import { observer } from 'mobx-react-lite';
import { Navigate, Route, Routes } from 'react-router-dom';
import "bootstrap/dist/css/bootstrap.min.css";
import NavBarComponent from './components/Navbar/NavbarComponent';
import NoMatchComponent from './pages/NoMatch/NoMatchComponent';
import HomeStore from './pages/Home/components/HomeStore';
import CatalogItemList from './pages/Home/components/HomeComponent';
import CatalogItem from './pages/CatalogItem';
import { CartStore } from './stores/Cart.store';
import CartComponent from './pages/Cart/CartComponent';
import OrderComponent from './pages/Order/OrderComponent';
import OrderHistoryComponent from './pages/Order/OrderHistoryComponent';
import AuthStore from './stores/Auth.store';
import Callback from './components/CallbackComponent';
import { OrderStore } from './pages/Order/order.store';

export const authStore = new AuthStore();
export const homeStore = new HomeStore();
export const myCart = new CartStore();
export const orderStore = new OrderStore();


const App: FC = observer(() => {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<NavBarComponent />}>
          <Route index element={<Navigate replace to="product" />} />
          <Route
            path="product"
            element={<CatalogItemList />} />
          <Route path="product/:id" element={<CatalogItem />} />
          <Route path="callback" element={<Callback />} />
          <Route path="do-order" element={<OrderComponent />} />
          {authStore.user && (
            <>
              <Route path="cart" element={<CartComponent />} />
              <Route path="orders" element={<OrderHistoryComponent />} />
            </>
          )}
          :
          {
            <>
              <Route path="orders" element={<Navigate replace to="/product" />} />
            </>
          }
        </Route>
        <Route path="*" element={<NoMatchComponent />} />
      </Routes>
    </div >
  );
});

export default App;
