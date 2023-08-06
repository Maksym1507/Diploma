import BasketItemModel from "./basketInfo"

interface BasketModel {
  basketItems: BasketItemModel[],
  totalSum: number
};

export default BasketModel