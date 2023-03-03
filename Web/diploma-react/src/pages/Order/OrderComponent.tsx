import { observer } from "mobx-react-lite";
import React, { FC, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Link, Navigate } from "react-router-dom";
import { authStore, myCart, orderStore } from "../../App";
import { config } from "../../constants/api-constants";
import OrderModel from "../../models/orderModel";


const OrderComponent: FC = observer(() => {
  const [orderMessage, setOrderMessage] = useState<number | null>();

  useEffect(() => {
    if (orderMessage) {
      if ((orderMessage as number)) {
        (async () => {
          await myCart.truncateBasket();
        })()
        myCart.totalSum = 0;
        alert(`Order was confirmed with id = ${orderMessage}`);
        <Navigate to="/orders" />;
      }
      else {
        alert("Failed to order");
      }
    }
  }, [orderMessage]);

  const {
    register,
    formState: { errors },
    handleSubmit,
  } = useForm({ mode: "onBlur" });

  const [formData, setFormData] = useState<OrderModel>({
    name: authStore.user?.profile.given_name,
    lastName: authStore.user?.profile.family_name,
    phoneNumber: "",
    email: "",
    country: "",
    region: "",
    city: "",
    address: "",
    index: "",
  } as OrderModel);

  function handleChange(e: any) {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  }

  const onSubmit = () => {
    (async () => {
      setOrderMessage(
        await orderStore.doOrder(
          {
            userId: authStore.user?.profile.sub,
            basketItems: myCart.items,
            name: formData.name,
            lastName: formData.lastName,
            phoneNumber: formData.phoneNumber,
            email: formData.email,
            country: formData.country,
            region: formData.region,
            city: formData.city,
            address: formData.address,
            index: formData.index,
            totalSum: myCart.totalSum
          } as OrderModel));
    })();
  };

  if (myCart.items.length > 0) {
    return (
      <div className="container p-2">
        <div className="row mx-2">
          <div className="pt-2 mb-2 col-md-5 col-lg-4 order-md-last bg-white rounded border border-dark">
            <h4 className="d-flex justify-content-between align-items-center mb-3 ">
              <span>Your cart</span>
              <span className="badge bg-dark rounded-pill">
                {myCart.items!.length}
              </span>
            </h4>
            <div className="container">
              <div className="row">
                <div className="col d-flex flex-column justify-content-center align-items-end">
                  <span>
                    <Link className="text-decoration-none" to="/cart">
                      <img
                        className="mb-1 me-1"
                        width={15}
                        height={15}
                        src="https://cdn-icons-png.flaticon.com/512/84/84380.png"
                        alt="edit"
                      />{" "}
                      Edit
                    </Link>
                  </span>
                </div>
              </div>
            </div>
            <div className="d-md-flex mt-2">
              <div
                className="overflow-auto pt-2 mb-3 mb-md-0"
                style={{ maxWidth: "500px", maxHeight: "310px" }}
              >
                {myCart.items!.map((product, index) => (
                  <>
                    <div className="card me-2 shadow-0 border mb-2">
                      <div className="card-body">
                        <div className="row">
                          <div className="col-md-2">
                            <img
                              key={index}
                              src={`${config.cdnHost}${product.pictureUrl}`}
                              className="img-fluid"
                              alt="product"
                            />
                          </div>
                          <div className="col-md-4 text-center d-flex justify-content-center align-items-center">
                            <p className="text-muted mb-0">
                              {product.title}
                            </p>
                          </div>
                          <div className="col-md-3 text-center d-flex justify-content-center align-items-center">
                            <p className="text-muted mb-0 small">
                              {product.count}
                            </p>
                          </div>
                          <div className="col-md-3 text-center d-flex justify-content-center align-items-center">
                            <p className="text-muted mb-0 small">
                              {product.price * product.count} $
                            </p>
                          </div>
                        </div>
                      </div>
                    </div>
                  </>
                ))}
              </div>
            </div>

            <h5 className="d-flex justify-content-end mt-3">
              Total: {myCart.totalSum} $
            </h5>
          </div>
          <div className="col-md-7 col-lg-8">
            <h4 className="mb-3">Placing an order</h4>
            <form
              className="needs-validation"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div className="row g-3">
                <div className="col-sm-4">
                  <label htmlFor="name" className="form-label">
                    Name
                  </label>
                  <input
                    {...register("name", {
                      required: "Name can not be empty",
                      minLength: { value: 3, message: "Min 3 symbols" },
                    })}
                    defaultValue={formData.name}
                    type="text"
                    className="form-control"
                    placeholder="Enter name"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.firstName && (
                      <p className="text-danger">
                        {errors?.name?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>

                <div className="col-sm-4">
                  <label htmlFor="lastName" className="form-label">
                    Surname
                  </label>
                  <input
                    {...register("lastName", {
                      required: "LastName can not be empty",
                      minLength: { value: 2, message: "Min 2 symbols" },
                    })}
                    defaultValue={formData.lastName}
                    type="text"
                    className="form-control"
                    placeholder="Enter lastName"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.lastName && (
                      <p className="text-danger">
                        {errors?.lastName?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>

                <div className="col-sm-4">
                  <label htmlFor="phoneNumber" className="form-label">
                    Phone number
                  </label>
                  <input
                    {...register("phoneNumber", {
                      required: "Phone can not be empty",
                      minLength: { value: 10, message: "Min 10 symbols" },
                    })}
                    type="tel"
                    className="form-control"
                    placeholder="Enter phone"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.phone && (
                      <p className="text-danger">
                        {errors?.phone?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>

                <div className="col-md-5">
                  <label htmlFor="email" className="form-label">
                    Email
                  </label>
                  <input
                    {...register("email", {
                      required: "Email can not be empty",
                      pattern: {
                        value: /^[a-zA-Z0-9].+@[a-zA-Z0-9]+\.[A-Za-z]+$/,
                        message: "Invalid email format",
                      },
                    })}
                    type="email"
                    className="form-control"
                    placeholder="Enter email"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.email && (
                      <p className="text-danger">
                        {errors?.email?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>

                <div className="col-md-4">
                  <label htmlFor="country" className="form-label">
                    Country
                  </label>
                  <select
                    {...register("country", {
                      required: "Country can not be empty",
                    })}
                    defaultValue={"DEFAULT"}
                    className="form-select"
                    id="country"
                    onChange={(e) => handleChange(e)}
                  >
                    <option disabled value="DEFAULT">
                      Выберите...
                    </option>
                    <option>Украина</option>
                  </select>
                </div>

                <div className="col-md-3">
                  <label htmlFor="region" className="form-label">
                    Region
                  </label>
                  <select
                    defaultValue={"DEFAULT"}
                    className="form-select"
                    id="region"
                    name="region"
                    onChange={(e) => handleChange(e)}
                  >
                    <option disabled value="DEFAULT">
                      Выберите...
                    </option>
                    <option>Харьковская</option>
                    <option>Киевская</option>
                    <option>Хмельницкая</option>
                    <option>Донецкая</option>
                    <option>Днепропетровская</option>
                    <option>Сумская</option>
                  </select>
                </div>

                <div className="col-md-3">
                  <label htmlFor="city" className="form-label">
                    City
                  </label>
                  <select
                    defaultValue={"DEFAULT"}
                    className="form-select"
                    id="city"
                    name="city"
                    onChange={(e) => handleChange(e)}
                  >
                    <option disabled value="DEFAULT">
                      Выберите...
                    </option>
                    <option>Харьков</option>
                    <option>Киев</option>
                    <option>Хмельницкий</option>
                    <option>Донецк</option>
                    <option>Днепр</option>
                    <option>Суммы</option>
                  </select>
                </div>

                <div className="col-md-5">
                  <label htmlFor="address" className="form-label">
                    Address
                  </label>
                  <input
                    {...register("address", {
                      required: "Address can not be empty",
                      minLength: {
                        value: 10,
                        message: "Address should contains minimum 10 symbols",
                      },
                    })}
                    type="text"
                    className="form-control"
                    placeholder="Enter address"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.address && (
                      <p className="text-danger">
                        {errors?.address?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>

                <div className="col-md-4">
                  <label htmlFor="index" className="form-label">
                    Index
                  </label>
                  <input
                    {...register("index", {
                      required: "Index can not be empty",
                      minLength: {
                        value: 5,
                        message: "Index should contains 5 symbols",
                      },
                      maxLength: {
                        value: 5,
                        message: "Index should contains 5 symbols",
                      },
                    })}
                    type="number"
                    className="form-control"
                    placeholder="Enter index"
                    onChange={(e) => handleChange(e)}
                  />
                  <div style={{ height: 20 }}>
                    {errors?.index && (
                      <p className="text-danger">
                        {errors?.index?.message?.toString()}
                      </p>
                    )}
                  </div>
                </div>
              </div>

              <hr className="my-4" />

              <button type="submit" className="w-100 btn btn-primary btn-lg">
                Confirm order
              </button>
            </form>
          </div>
        </div>
      </div>
    );
  }
  else {
    return <Navigate to="/product" />;
  }
});

export default OrderComponent;
