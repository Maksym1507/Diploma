import { WebStorageStateStore } from "oidc-client";

export const config = {
  authority: 'http://www.alevelwebsite.com:5002',
  client_id: 'client_pkce',
  client_secret: "secret",
  redirect_uri: `http://www.alevelwebsite.com/callback`,
  response_type: 'code',
  scope: 'openid profile basket.basketbff order.orderbff',
  post_logout_redirect_uri: `http://www.alevelwebsite.com`,
  useStore: new WebStorageStateStore({ store: window.localStorage }),
  automaticSilentRenew: true,
  monitorSession: true,
  checkSessionInterval: 2000,
  silent_redirect_uri: `http://www.alevelwebsite.com`,
  filterProtocolClaims: true,
  revokeAccessTokenOnSignout: true,
  revokeRefreshTokenOnSignout: true,
  usePkce: true,
};