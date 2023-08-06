import { FC, ReactElement } from "react";
import { Button, Card } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { authStore, myCart } from "../../../App";
import { config } from "../../../constants/api-constants";
import { CatalogItemModel } from "../../../models/catalogItemModel";
import { CatalogItemDto } from "../../../models/dtos/catalogItemsDto";

const CatalogItemCard: FC<CatalogItemDto> = (props): ReactElement => {

  const navigation = useNavigate();
  return (
    <div className="col">
      <Card className="mt-1 ms-4 h-100">
        <Card.Img
          variant="top"
          src={`${config.cdnHost}${props.pictureUrl}`}
          alt={props.title}
          onClick={() => {
            navigation(`/product/${props.id}`);
          }}
        />
        <Card.Body>
          <Card.Title>{props.title}</Card.Title>
          <Card.Text>{props.price} ₴</Card.Text>
          {authStore.user && <Button
            className="btn-info d-flex"
            onClick={async () =>
              await myCart.addUpdateItem({
                id: props.id,
                title: props.title,
                description: props.description,
                price: props.price,
                weight: props.weight,
                pictureUrl: props.pictureUrl,
                catalogTypeId: props.catalogType.id,
                availableStock: props.availableStock
              } as CatalogItemModel)
            }
          >
            Add to cart
          </Button>}
        </Card.Body>
      </Card>
    </div>
  );
}

export default CatalogItemCard
