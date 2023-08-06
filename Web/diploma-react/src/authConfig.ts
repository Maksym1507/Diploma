import { UserManager } from 'oidc-client-ts'

export const userManager = new UserManager({
  authority: "http://www.alevelwebsite.com:5002",
  client_id: "react_pkce",
  client_secret: "secret",
  redirect_uri: "http://localhost:3000/signin-callback.html",
  response_type: "code",
  scope: "openid profile website",
  post_logout_redirect_uri: "http://localhost:3000",
  filterProtocolClaims: true,
  loadUserInfo: true,

});