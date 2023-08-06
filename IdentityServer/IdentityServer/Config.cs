using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("alevelwebsite.com")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("website")
                    }
                },
                new ApiResource("catalog")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("catalog.catalogitem"),
                        new Scope("catalog.catalogtype")
                    }
                },
                new ApiResource("basket")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("basket.basketbff"),
                        new Scope("basket.bf"),
                    }
                },
                new ApiResource("order")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("order.orderbff"),
                        new Scope("order.orderitem")
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client_pkce",
                    ClientName = "React PKCE Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = { $"{configuration["MvcUrl"]}/callback" },
                    PostLogoutRedirectUris = { $"{configuration["MvcUrl"]}" },
                    AllowedCorsOrigins =
                    {
                        $"{configuration["MvcUrl"]}"
                    },

                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "basket.basketbff",
                        "order.orderbff"
                    },
                },
                new Client
                {
                    ClientId = "catalog",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                },
                new Client
                {
                    ClientId = "catalogswaggerui",
                    ClientName = "Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["CatalogApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["CatalogApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "website", "catalog.catalogitem", "catalog.catalogtype"
                    }
                },
                new Client
                {
                    ClientId = "basket",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientId = "basketswaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "basket.basketbff",
                        "order.orderbff"
                    }
                },
                new Client
                {
                    ClientId = "order",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                },
                new Client
                {
                    ClientId = "orderswaggerui",
                    ClientName = "Order Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["OrderApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["OrderApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "order.orderbff", "order.orderitem"
                    }
                },
            };
        }
    }
}
