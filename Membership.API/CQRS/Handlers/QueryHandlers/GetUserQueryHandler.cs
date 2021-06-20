using MediatR;
using Membership.API.CQRS.Queries.Request;
using Membership.API.CQRS.Queries.Response;
using Membership.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Handlers.QueryHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQueryRequest, GetUserQueryResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public GetUserQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new GetUserQueryResponse
                {
                    IsSuccess = false,
                    Message = "No User",
                    Email = request.Email,
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (!result.Succeeded)
            {
                return new GetUserQueryResponse
                {
                    IsSuccess = false,
                    Message = "Password is wrong",
                    Email = request.Email,
                };
            }

            return new GetUserQueryResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                Email = request.Email,
            };

        }
    }
}
