﻿using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRespository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IAuthenticationService authenticationService, 
            IUserRespository userRepository, 
            IUnitOfWork unitOfWork)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                new FirstName(request.FirstName),
                new LastName(request.LastName), 
                new Email(request.Email));

            var identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);

            user.SetIdentityId(identityId);

            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(user.Id);
        }
    }
}