import { makeAutoObservable } from "mobx";
import { User, UserManager } from "oidc-client";
import { config } from "../constants/config";

class AuthStore {
    user: User | null = null;
    oidc_client: UserManager;

    constructor() {
        this.oidc_client = new UserManager(config);
        makeAutoObservable(this);
        this.oidc_client.events.addUserLoaded((user) => {
            if (user) {
                this.user = user;
            }
        });
    };

    login() {
        this.oidc_client.signinRedirect();
    };

    logout() {
        this.oidc_client.signoutRedirect();
        this.user = null;
    };

    handleCallback() {
        this.oidc_client.signinRedirectCallback().then(response => {
            this.user = response;
        }).catch(error => {
            console.log(error);
        });
    }
};

export default AuthStore;