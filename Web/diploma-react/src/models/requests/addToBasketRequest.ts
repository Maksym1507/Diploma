import { ProductToBasketModel } from "../productToBasketModel"

export interface IAddToBasketRequest {
  id: string
  product: ProductToBasketModel
}