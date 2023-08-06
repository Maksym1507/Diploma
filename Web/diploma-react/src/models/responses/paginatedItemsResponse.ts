import { CatalogItemDto } from "../dtos/catalogItemsDto";

export interface PaginatedItemsResponse<T> {
  'pageIndex': number,
  'pageSize': number,
  'count': number,
  'data': T[]
}