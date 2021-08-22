﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models.Dto
{
    public class ClientDto
    {

        public ClientDto()
        {
            AllowedScopes = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            RedirectUris = new List<string>();
            IdentityProviderRestrictions = new List<string>();
            AllowedCorsOrigins = new List<string>();
            AllowedGrantTypes = new List<string>();
            Claims = new List<ClientClaimDto>();
            Properties = new List<ClientPropertyDto>();
        }

        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        public int AccessTokenLifetime { get; set; } = 3600;
        public int? ConsentLifetime { get; set; }
        public string AccessTokenType { get; set; } //Jwt or Reference
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowRememberConsent { get; set; } = true;
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; } = 300;
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        public string ClientUri { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public int IdentityTokenLifetime { get; set; } = 300;
        public bool IncludeJwtId { get; set; }
        public string LogoUri { get; set; }
        public string ClientClaimsPrefix { get; set; } = "client_";
        public string PairWiseSubjectSalt { get; set; }
        public string ProtocolType { get; set; } = "oidc";
        public int RefreshTokenExpiration { get; set; } = 1;
        public int RefreshTokenUsage { get; set; } = 1;
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        public bool RequireClientSecret { get; set; } = true;
        public bool RequireConsent { get; set; } = true;
        public bool RequirePkce { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public List<string> PostLogoutRedirectUris { get; set; }

        public List<string> IdentityProviderRestrictions { get; set; }

        public List<string> RedirectUris { get; set; }

        public List<string> AllowedCorsOrigins { get; set; }

        public List<string> AllowedGrantTypes { get; set; }

        public List<string> AllowedScopes { get; set; }

        public List<ClientClaimDto> Claims { get; set; }
        public List<ClientPropertyDto> Properties { get; set; }

        public ClientSecretDto ClientSecret { get; set; }

        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; } = 300;

        public bool RequireRequestObject { get; set; }

        public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; }

    }
}