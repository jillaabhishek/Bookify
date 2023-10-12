﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthenticationOptions _authenticationOptions;

        public JwtBearerOptionsSetup(IOptions<AuthenticationOptions> authenticationOptions)
        {
            _authenticationOptions = authenticationOptions.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Audience = _authenticationOptions.Audience;
            options.MetadataAddress = _authenticationOptions.MetadataUrl;
            options.RequireHttpsMetadata = _authenticationOptions.RequireHttpsMetadata;
            options.TokenValidationParameters.ValidIssuer = _authenticationOptions.Issuer;
        }
    }
}
