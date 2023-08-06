import { CatalogTypeDto } from "./catalogTypeDto";

export interface CatalogItemDto {
  'id': number,
  'title': string,
  'description': string,
  'price': number,
  'weight': number,
  'pictureUrl': string,
  'catalogType': CatalogTypeDto,
  'availableStock': number
}