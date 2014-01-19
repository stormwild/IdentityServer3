﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.Core.Connect.Models;
using Thinktecture.IdentityServer.Core.Connect.Services;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityServer.Core.Connect.TestServices
{
    public class TestCoreSettings : ICoreSettings
    {
        private static readonly List<Client> _clients;

        static TestCoreSettings()
        {
            _clients = new List<Client>
            {
                new Client
                {
                    ClientName = "Code Client",
                    ClientId = "codeclient",
                    ClientSecret = "secret",
                    Flow = Flows.Code,
                    ApplicationType = ApplicationTypes.Web,
                    RequireConsent = false,
                    
                    RedirectUris = new List<Uri>
                    {
                        new Uri("https://localhost/cb"),
                        new Uri("https://localhost:44309/oidccallback")
                    },
                    
                    ScopeRestrictions = new List<string>
                    { 
                        Constants.StandardScopes.Profile 
                    },

                    SigningKeyType = SigningKeyTypes.ClientSecret,
                    SubjectType = SubjectTypes.Global,
                    
                    IdentityTokenLifetime = 360,
                    AccessTokenLifetime = 360,
                },
                new Client
                {
                    ClientName = "Implicit Client",
                    ClientId = "implicitclient",
                    ClientSecret = "secret",
                    Flow = Flows.Implicit,
                    ApplicationType = ApplicationTypes.Web,
                    RequireConsent = false,
                    
                    RedirectUris = new List<Uri>
                    {
                        new Uri("https://localhost/cb")
                    },
                    
                    ScopeRestrictions = new List<string>
                    { 
                        Constants.StandardScopes.Profile 
                    },

                    SigningKeyType = SigningKeyTypes.ClientSecret,
                    SubjectType = SubjectTypes.Global,
                    
                    IdentityTokenLifetime = 360,
                    AccessTokenLifetime = 360,
                }
            };
        }

        public Client FindClientById(string clientId)
        {
            return (from c in _clients
                    where c.ClientId == clientId
                    select c).SingleOrDefault();
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new Scope[]{
                 new Scope{
                     Name = Constants.StandardScopes.Profile, 
                     Description = "Standard Profile",
                     Claims = (Constants.ScopeToClaimsMapping[Constants.StandardScopes.Profile].Select(x=>new ScopeClaim{Name = x}))
                },
                 new Scope{
                     Name = Thinktecture.IdentityServer.Core.Constants.StandardScopes.Email, 
                     Claims = (Constants.ScopeToClaimsMapping[Constants.StandardScopes.Email].Select(x=>new ScopeClaim{Name = x}))
                },
             };
        }

        public bool RequiresConsent(string clientId, ClaimsPrincipal user, IEnumerable<string> scopes)
        {
            return false;
        }

        public System.Security.Cryptography.X509Certificates.X509Certificate2 GetSigningCertificate()
        {
            return X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=sts", false).First();
        }

        public Uri GetIssuerUri()
        {
            return new Uri("https://idsrv3.com");
        }


        public string GetSiteName()
        {
            return "tt.IdSrv 3";
        }
    }
}
