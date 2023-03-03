import { observer } from "mobx-react-lite";
import React, { FC } from "react";
import { Accordion, Spinner } from "react-bootstrap";
import { orderStore } from "../../App";

const OrderHistoryComponent: FC = observer(() => {
  return (
    <>
      <div className="mb-3">
        {orderStore.isLoading ? (
          <div className="container">
            <div className="row min-vh-100">
              <div className="col d-flex flex-column justify-content-center align-items-center">
                <Spinner animation="border" />
              </div>
            </div>
          </div>
        ) : (
          <>
            <h2>My orders</h2>
            {orderStore.orders.length ? (
              (orderStore.orders).slice()
                .sort((a, b) => {
                  return b.id - a.id;
                })
                .map((order, index) => (
                  <div key={index}>
                    <Accordion className="container w-75 mb-1">
                      <Accordion.Item eventKey="0" className="">
                        <Accordion.Header className="d-flex justify-content-between align-items-center">
                          <div className="col-sm-3 me-2 text-center">
                            Order №{order.id}
                          </div>
                          <div className="col-sm-5 text-center">
                            <p className="mb-0">Order price: {order.totalSum}₴</p>
                          </div>
                          <div className="col-sm-3 ms-3 text-end">
                            {" "}
                          </div>
                        </Accordion.Header>
                        <Accordion.Body className="w-100">
                          <section className=" gradient-custom">
                            <div className="container">
                              <div className="row d-flex justify-content-center align-items-center h-100">
                                <div>
                                  <div className="card">
                                    <div className="card-header px-2 py-3">
                                      <h5 className="text-muted mb-0">Order details</h5>
                                    </div>
                                    <div className="card-body pe-4 ps-4 pb-1">
                                      <div className="d-flex justify-content-between pt-1 ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Country: {order.country}
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Region: {order.region} область
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          City: {order.city}
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Address: {order.address}
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Fullname: {order.name} {order.lastName}
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Email: {order.email}
                                        </p>
                                      </div>
                                      <div className="d-flex justify-content-between ps-1">
                                        <p className="text-muted mb-0 text-start">
                                          Phone: {order.phoneNumber}
                                        </p>
                                      </div>
                                    </div>
                                    <div className="card-footer border-0 px-4 py-3 bg-secondary">
                                      <h5 className="d-flex align-items-center justify-content-end text-white text-uppercase mb-0">
                                        Total:{" "}
                                        <span className="h3 mb-0 ms-2">
                                          {order.totalSum} ₴
                                        </span>
                                      </h5>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </section>
                        </Accordion.Body>
                      </Accordion.Item>
                    </Accordion>
                  </div>
                ))
            ) : (
              <div>You haven't ordered anything yet</div>
            )}
          </>
        )}
      </div>
    </>
  );
});

export default OrderHistoryComponent;
