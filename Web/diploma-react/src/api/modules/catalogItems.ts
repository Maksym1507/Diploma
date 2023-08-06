import { config } from "../../constants/api-constants";
import { ItemsByTypeRequest } from "../../models/requests/itemsByTypeRequest";
import apiClient from "../client";

export const getCatalogItemById = (id: string) =>
  apiClient({
    url: config.catalogUrl,
    path: `itemById/${id}`,
    method: "POST",
  });

export const getCatalogItems = (pageIndex: number, pageSize: number, filter: object) =>
  apiClient({
    url: config.catalogUrl,
    path: `items`,
    method: "POST",
    data: { pageIndex, pageSize, filter }
  });

export const getCatalogItemsByType = (request: ItemsByTypeRequest) =>
  apiClient({
    url: config.catalogUrl,
    path: `itemsByType`,
    method: "POST",
    data: request
  });

export const getTypes = () =>
  apiClient({
    url: config.catalogUrl,
    path: `types`,
    method: "POST",
  });