using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authentication
{
    public class JwtService : IJwtService
    {
        private static readonly Error AuthenticationFailed = new(
            "Keybloak.AuthenticationFailed", "Failed to acquire access token do to authentication failure");

        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _keycloakOptions;

        public JwtService(IOptions<KeycloakOptions> keycloakOptions, HttpClient httpClient)
        {
            _keycloakOptions = keycloakOptions.Value;
            _httpClient = httpClient;
        }

        public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var authReqestParameters = new KeyValuePair<string, string>[]
                {
                    new("client_id", _keycloakOptions.AuthClientId),
                    new("client_secret", _keycloakOptions.AuthClientSecret),
                    new("scope", "openid email"),
                    new("username", email),
                    new("password", password),
                    new("grant_type","password")
                };

                var authorizationRequestContent = new FormUrlEncodedContent(authReqestParameters);
                var response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);

                response.EnsureSuccessStatusCode();

                var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>();

                if (authorizationToken is null)
                    return Result.Failure<string>(AuthenticationFailed);

                return Result.Success(authorizationToken.AccessToken);

            }
            catch (HttpRequestException ex)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }
        }
    }
}
