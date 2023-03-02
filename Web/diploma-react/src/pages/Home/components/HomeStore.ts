import { makeAutoObservable, runInAction } from "mobx";
import * as catalogItemsApi from "../../../api/modules/catalogItems";
import { CatalogItemDto } from "../../../models/dtos/catalogItemsDto";

class HomeStore {
  singleCatalogItem: CatalogItemDto = {} as CatalogItemDto;
  currentPage = 1;
  totalPages = 0;
  pageSize = 6;
  filter: {
    "Type": number | null;
  };
  items: CatalogItemDto[] = [];
  isLoading = false;

  constructor() {
    makeAutoObservable(this);
    this.filter = null!;
    runInAction(this.prefetchData);
  }

  prefetchData = async () => {
    try {
      this.isLoading = true;
      const pageIndex = this.currentPage - 1;
      const res = await catalogItemsApi.getCatalogItems(pageIndex, this.pageSize, this.filter);
      this.items = res.data;
      console.log(res);
      this.totalPages = Math.ceil(res.count / this.pageSize);
    } catch (e) {
      if (e instanceof Error) {
        console.error(e.message);
      }
    }
    this.isLoading = false;
  };

  async getSingleCatalogItem(id: string) {
    try {
      this.isLoading = true;
      const res = await catalogItemsApi.getCatalogItemById(id);
      this.singleCatalogItem = res;
      console.log(res);
    } catch (e) {
      if (e instanceof Error) {
        console.error(e.message);
      }
    }
    this.isLoading = false;
  }
}

export default HomeStore;
