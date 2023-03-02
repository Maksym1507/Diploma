import BasketItemModel from "./basketInfo";

interface OrderModel {
  userId: string;
  basketItems: BasketItemModel[]
  name: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  country: string;
  region: string;
  city: string;
  address: string;
  index: string;
  totalSum: number;
};

export default OrderModel;