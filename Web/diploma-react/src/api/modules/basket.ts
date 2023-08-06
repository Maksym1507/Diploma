import { config } from "../../constants/api-constants";
import { IAddToBasketRequest } from "../../models/requests/addToBasketRequest";
import apiClient from "../client";

export const addItemForBasket = (addToBasketRequest: IAddToBasketRequest) =>
  apiClient({
    url: config.basketUrl,
    path: `additemtobasket`,
    method: "POST",
    data: addToBasketRequest
  });

export const getBasket = (id: string) =>
  apiClient({
    url: config.basketUrl,
    path: `get`,
    method: "POST",
    data: { userId: id }
  });

export const deleteBasketItem = (userId: string, basketItemId: number) =>
  apiClient({
    url: config.basketUrl,
    path: `deleteitemfrombasket`,
    method: "POST",
    data: { userId: userId, basketItemId: basketItemId }
  });

export const truncateBasket = (userId: string) =>
  apiClient({
    url: config.basketUrl,
    path: `truncatebasket`,
    method: "POST",
    data: { userId: userId }
  });