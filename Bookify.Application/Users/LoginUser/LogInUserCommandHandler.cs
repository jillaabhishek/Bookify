using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookify.Application.Users.LoginUser
{
    internal sealed class LogInUserCommandHandler : ICommandHandler<LoginUserCommand, AccessTokenResponse>
    {
        private readonly IJwtService _jwtService;

        public LogInUserCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<Result<AccessTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _jwtService.GetAccessTokenAsync(request.Email, request.Password, cancellationToken);

            if (result.IsFailure)
                return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);

            return Result.Success(new AccessTokenResponse(result.Value));
        }
    }
}
