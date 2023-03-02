import React from "react";
import { Link } from "react-router-dom";

function NoMatchComponent() {
  return (
    <div className="container">
      <div className="row min-vh-100">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          <h2>Page not found...</h2>

          <p>
            <Link to="/product">Go to catalog</Link>
          </p>
        </div>
      </div>
    </div>
  );
}

export default NoMatchComponent;