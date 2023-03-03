import { observer } from "mobx-react-lite";
import { FC, ReactElement, useEffect } from "react";
import { Button } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { authStore, homeStore, myCart } from "../../App";
import { config } from "../../constants/api-constants";
import { CatalogItemModel } from "../../models/catalogItemModel";
import NoMatchComponent from "../NoMatch/NoMatchComponent";

const User: FC<any> = observer((): ReactElement => {
  const { id } = useParams();
  const navigation = useNavigate();

  useEffect(() => {
    (async () => {
      if (id) {
        await homeStore.getSingleCatalogItem(id);
      }
    })();
  }, [id]);

  if (homeStore.singleCatalogItem) {
    if (homeStore.singleCatalogItem.id) {
      return (
        <>
          <section className="my-4">
            <div className="container px-4 px-lg-5">
              <div className="row gx-4 gx-lg-5">
                <div className="col-md-4">
                  <img
                    className="card-img-top mb-5 mb-md-0"
                    src={`${config.cdnHost}${homeStore.singleCatalogItem.pictureUrl}`}
                    alt={homeStore.singleCatalogItem.title}
                  />
                </div>
                <div className="col-md-6">
                  <h1 className="display-4 fw-bolder mb-2">{homeStore.singleCatalogItem.title}</h1>
                  <div className="fs-5 mb-2">
                    <span>{homeStore.singleCatalogItem.price} ₴</span>
                  </div>
                  <p className="mb-3"><span className="fw-bolder">Ingredients: </span>{homeStore.singleCatalogItem.description}</p>
                  <div className="d-flex mb-2"><span className="fw-bolder">Weight:&nbsp;</span>{homeStore.singleCatalogItem.weight}g</div>
                  <div className="d-flex mt-5">
                    <Button
                      onClick={() => {
                        navigation(-1);
                      }}
                      className="me-3 btn-info"
                    >
                      Back to shop
                    </Button>
                    {authStore.user && <button
                      onClick={async () =>
                        await myCart.addUpdateItem({
                          id: homeStore.singleCatalogItem.id,
                          title: homeStore.singleCatalogItem.title,
                          description: homeStore.singleCatalogItem.description,
                          price: homeStore.singleCatalogItem.price,
                          weight: homeStore.singleCatalogItem.weight,
                          pictureUrl: homeStore.singleCatalogItem.pictureUrl,
                          catalogTypeId: homeStore.singleCatalogItem.catalogType.id,
                          availableStock: homeStore.singleCatalogItem.availableStock
                        } as CatalogItemModel)
                      }
                      className="btn btn-outline-dark flex-shrink-0"
                    >
                      Add to cart
                    </button>}
                  </div>
                </div>
              </div>
            </div>
          </section>
        </>
      );
    }
  }
  return <NoMatchComponent />;
});

export default User;
