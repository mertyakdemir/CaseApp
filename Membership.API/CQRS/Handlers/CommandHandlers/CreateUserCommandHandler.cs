using MediatR;
using Membership.API.CQRS.Commands.Request;
using Membership.API.CQRS.Commands.Response;
using Membership.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Handlers.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<User> _userManager;

        public CreateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                GenerateRef = request.GenerateRef,
                RefCode = request.RefCode,
                RefUri = request.RefUri
            };

            await _userManager.CreateAsync(user, request.Password);

            return new CreateUserCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
