import React, { ReactElement, FC } from "react";
import { observer } from "mobx-react-lite";
import { homeStore } from "../../../App";
import { Spinner } from "react-bootstrap";
import CatalogItemCard from "./CatalogItemCard";

const CatalogItemList: FC<any> = observer((): ReactElement => {

  const nextPage = async () => {
    if (homeStore.currentPage < homeStore.totalPages) {
      console.log(homeStore.totalPages);

      homeStore.currentPage++;
      await homeStore.prefetchData();
    }
  };

  const previousPage = async () => {
    if (homeStore.currentPage !== 1) {
      homeStore.currentPage--;
      await homeStore.prefetchData();
    }
  };

  return (
    <>
      <div className="container-sm">
        {homeStore.isLoading ? (
          <div className="container">
            <div className="row min-vh-100">
              <div className="col d-flex flex-column justify-content-center align-items-center">
                <Spinner animation="border" />
              </div>
            </div>
          </div>
        ) : (
          <>
            <div className="row row-cols-1 row-cols-md-3 g-4 mb-5">
              {homeStore.items.map((item) => (
                <CatalogItemCard
                  key={item.id}
                  id={item.id}
                  title={item.title}
                  description={item.description}
                  price={item.price}
                  weight={item.weight}
                  pictureUrl={item.pictureUrl}
                  catalogType={item.catalogType}
                  availableStock={item.availableStock}
                />
              ))
              }
            </div>
            <nav>
              <ul className="pagination justify-content-center align-center">
                <li className="page-item">
                  <a className="page-link" href="#" onClick={previousPage}>
                    &lt;
                  </a>
                </li>
                <li className="page-item">
                  <a className="page-link">{homeStore.currentPage}</a>
                </li>
                <li className="page-item">
                  <a className="page-link" href="#" onClick={nextPage}>
                    &gt;
                  </a>
                </li>
              </ul>
            </nav>
          </>
        )}
      </div>
    </>
  );
});

export default CatalogItemList;
