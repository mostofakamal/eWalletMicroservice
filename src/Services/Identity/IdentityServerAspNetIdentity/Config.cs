
using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(Scopes.Kyc),
                new ApiScope(Scopes.Transaction),
                new ApiScope(Scopes.Reward),
                new ApiScope(Scopes.Notification)
            };
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(Scopes.Kyc, "Kyc Service")
                {
                    Scopes = { Scopes.Kyc }
                },
                new ApiResource(Scopes.Transaction, "Transaction Service")
                {
                    Scopes = { Scopes.Transaction }
                },
                new ApiResource(Scopes.Reward, "Reward Service")
                {
                    Scopes = { Scopes.Reward }
                },
                new ApiResource(Scopes.Notification, "Notification Service")
                {
                    Scopes = { Scopes.Notification }
                },

            };
        }


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "ewalletWeb",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", Scopes.Kyc,Scopes.Transaction,Scopes.Reward,Scopes.Notification }
                },
                new Client
                {
                    ClientId = "ewalletclient",
                    ClientSecrets = new [] { new Secret("ewalletSecret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { Scopes.Kyc,Scopes.Transaction,Scopes.Reward,Scopes.Notification}
                },
            };
    }

    public class Scopes
    {
        public const string Kyc = "kyc";
        public const string Transaction = "transaction";
        public const string Reward = "reward";
        public const string Notification = "notification";
    }
}