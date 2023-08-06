import { makeAutoObservable, runInAction } from "mobx";
import OrderModel from "../../models/orderModel";
import OrderResponse from "../../models/responses/orderResponse";
import * as orderApi from "../../api/modules/order";
import { authStore } from "../../App";

export class OrderStore {
  orders: OrderResponse[] = [];
  isLoading = false;
  constructor() {
    makeAutoObservable(this);
    runInAction(this.prefetchData);
  }

  prefetchData = async () => {
    try {
      this.isLoading = true;
      const res = await orderApi.getOrderByUserId(authStore.user?.profile.sub ?? "default");
      this.orders = res;
    } catch (e) {
      if (e instanceof Error) {
        console.error(e.message);
      }
    }
    this.isLoading = false;
  };

  doOrder = async (order: OrderModel) => {
    var doOrderResponse = await orderApi.doOrder(order);
    await this.prefetchData();
    return doOrderResponse;
  }
}
