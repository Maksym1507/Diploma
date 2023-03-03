import { observer } from "mobx-react-lite";
import React, { FC } from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import { Link, Outlet } from "react-router-dom";
import { authStore, myCart, orderStore } from "../../App";

function getTotoalCountOfBasketItems() {
  return myCart.items.reduce((accu, item) => accu + item.count, 0);
}

const NavBarComponent: FC = observer(() => {
  return (
    <>
      <Navbar sticky="top" bg="secondary" expand="lg" variant="dark">
        <Container fluid>
          <Navbar.Brand>Pizza Life</Navbar.Brand>
          <Navbar.Toggle aria-controls="navbarScroll" />
          <Navbar.Collapse id="navbarScroll">
            <Nav
              className="me-auto my-2 my-lg-0"
              style={{ maxHeight: "100px" }}
              navbarScroll
            >
              <Nav.Link
                as={Link}
                to="/product"
                className="text-decoration-none text-white cursor-pointer"
              >
                Products
              </Nav.Link>
              {authStore.user && <Nav.Link as={Link} to="/cart" onClick={async () => await myCart.prefetchData()}>
                <img
                  src="https://cdn-icons-png.flaticon.com/512/118/118089.png"
                  alt=""
                  width={20}
                  className="cursor-pointer"
                />
                <span className="ms-1">{getTotoalCountOfBasketItems()}</span>
              </Nav.Link>
              }
              {authStore.user && (
                <Nav.Link className="" as={Link} to="orders" onClick={async () => await orderStore.prefetchData()}>
                  <img
                    src="https://cdn-icons-png.flaticon.com/512/5885/5885642.png"
                    alt=""
                    width={25}
                    className="cursor-pointer"
                  />
                </Nav.Link>
              )}
            </Nav>
            {!authStore.user && (
              <Nav.Link
                className="text-white me-3"
                onClick={() => authStore.login()
                }
              >
                Login
              </Nav.Link>)}
            {authStore.user && (
              <Nav.Link
                className="text-white me-3"
                onClick={() => authStore.login()
                }
              >
                {authStore.user.profile.name}
              </Nav.Link>)}
            {authStore.user && (
              <Nav.Link
                className="text-white me-3"
                onClick={() => authStore.logout()
                }
              >
                <img
                  src="../images/logout.png"
                  alt=""
                  width={25}
                  className="cursor-pointer"
                />
              </Nav.Link>
            )}
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <Outlet />
    </>
  );
});

export default NavBarComponent;