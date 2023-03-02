import { makeAutoObservable, runInAction } from "mobx";
import * as cartApi from "../api/modules/basket";
import { authStore } from "../App";
import BasketItemModel from "../models/basketInfo";
import { ProductToBasketModel } from "../models/productToBasketModel";

export class CartStore {
  items: BasketItemModel[] = [];
  totalSum: number = 0;
  isLoading = false;
  constructor() {
    makeAutoObservable(this);
    runInAction(this.prefetchData);
  }

  prefetchData = async () => {
    try {
      this.isLoading = true;
      const res = await cartApi.getBasket(authStore.user?.profile.sub ?? "default");
      this.items = res.basketItems;
      this.totalSum = res.totalSum;
    } catch (e) {
      if (e instanceof Error) {
        console.error(e.message);
      }
    }
    this.isLoading = false;
  };

  addUpdateItem = async (item: ProductToBasketModel) => {
    await cartApi.addItemForBasket({ id: authStore.user?.profile.sub!, product: { id: item.id, title: item.title, price: item.price, pictureUrl: item.pictureUrl } });
    await this.prefetchData();
  }

  deleteItem = async (id: number) => {
    await cartApi.deleteBasketItem(authStore.user?.profile.sub!, id);
    await this.prefetchData();
  }

  truncateBasket = async () => {
    await cartApi.truncateBasket(authStore.user?.profile.sub!);
    await this.prefetchData();
  }
}
