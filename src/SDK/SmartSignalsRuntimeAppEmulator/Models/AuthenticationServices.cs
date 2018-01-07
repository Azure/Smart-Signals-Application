﻿//-----------------------------------------------------------------------
// <copyright file="AuthenticationServices.cs" company="Microsoft Corporation">
//        Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Azure.Monitoring.SmartSignals.Emulator.Models
{
    using System;
    using System.Linq;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// A class providing methods used to authenticate the user with their organization's AAD.
    /// </summary>
    public class AuthenticationServices 
    {
        // The authorization authority used for multi-tenant applications authorizations.
        private const string CommonAuthority = "https://login.microsoftonline.com/common";

        // The resource ID used for authentication requests.
        //private const string ResourceId = "https://graph.windows.net/";
        private const string ResourceId = "https://management.azure.com/";

        // The client ID for the emulator's application - this is registered with Azure, so changing it will break all
        // authentications.
        private const string ClientId = "7696b566-f71e-450a-8681-3b43cec4bef4";

        // The redirect URI registered in Azure for the emulator's application - changing it will break all
        // authentications.
        private static readonly Uri RedirectUri = new Uri("https://azuresmartsignals.microsoft.com");

        // The authentication context used to authenticate with AAD
        private readonly AuthenticationContext authenticationContext;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event UserAuthenticatedEventHandler UserAuthenticated;

        public AuthenticationResult AuthenticationResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationServices"/> class.
        /// </summary>
        public AuthenticationServices()
        {
            // Initialize the AuthenticationContext with the common (tenant-less) endpoint
            this.authenticationContext = new AuthenticationContext(CommonAuthority);

            // If we already have tokens in the cache
            if (this.authenticationContext.TokenCache.ReadItems().Any())
            {
                // Re-bind the AuthenticationContext to the authority that sourced the token in the cache.
                // This is needed for the cache to work when asking a token from that authority
                // (the common endpoint never triggers cache hits)
                string cachedAuthority = this.authenticationContext.TokenCache.ReadItems().First().Authority;
                this.authenticationContext = new AuthenticationContext(cachedAuthority);
            }
        }

        #region Implementation of IAuthenticationServices

        /// <summary>
        /// Authenticates the user with their organization's AAD.
        /// </summary>
        //public async void AuthenticateUserAsync()
        //{
        //    this.authenticationContext.TokenCache.Clear();
        //    // Get a token for the web API and in so doing present the user with the consent experience
        //    AuthenticationResult ar = await this.authenticationContext.AcquireTokenAsync(ResourceId, ClientId, new UserCredential())/*RedirectUri, new PlatformParameters(PromptBehavior.Auto, null)*/;
        //    this.UserAuthenticated?.Invoke(this, new UserAuthenticatedEventArgs(ar.UserInfo));
        //}

        /// <summary>
        /// Authenticates the user with their organization's AAD.
        /// </summary>
        public AuthenticationResult AuthenticateUser()
        {
            this.authenticationContext.TokenCache.Clear();
            // Get a token for the web API and in so doing present the user with the consent experience
            var pb = PromptBehavior.Auto;
            this.AuthenticationResult = this.authenticationContext.AcquireToken(ResourceId, ClientId, RedirectUri, pb);
            //this.UserAuthenticated?.Invoke(this, new UserAuthenticatedEventArgs(ar.UserInfo));

            return this.AuthenticationResult;
        }

        #endregion
    }
}
