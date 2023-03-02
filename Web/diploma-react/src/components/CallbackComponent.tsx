import React, { FC, ReactElement, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { authStore } from "../App";


const Callback: FC<any> = (): ReactElement => {
    var navigate = useNavigate();

    useEffect(() => {
        async function handleCallback() {
            authStore.handleCallback()
            navigate('/');
        }
        handleCallback();
    }, []);

    return (
        <>
            Login
        </>
    );
}

export default Callback; 