import { CatalogItemModel } from "../catalogItemModel";

interface OrderDetailsResponse {
  product: CatalogItemModel
  count: number;
};

export default OrderDetailsResponse;