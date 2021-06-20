using MediatR;
using Membership.API.CQRS.Queries.Request;
using Membership.API.CQRS.Queries.Response;
using Membership.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Handlers.QueryHandlers
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQueryRequest, GetByIdUserQueryResponse>
    {
        private readonly UserManager<User> _userManager;

        public GetByIdUserQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new GetByIdUserQueryResponse
                {
                    IsSuccess = false,
                    Message = "No User",
                    Email = request.Email,
                };
            }

            var refCode = await _userManager.Users.Where(x => x.GenerateRef == user.GenerateRef).FirstOrDefaultAsync();

            var generateRef = refCode.GenerateRef;

            var myRefs = await _userManager.Users.Where(x => x.RefCode == generateRef).ToListAsync();

            return new GetByIdUserQueryResponse()
            {
                IsSuccess = true,
                Email = user.Email,
                RefCode = user.GenerateRef,
                RefUri = user.RefUri,
                FirstName = myRefs.Select(x => x.FirstName.Substring(0, 1).ToUpper()).ToList(),
                LastName = myRefs.Select(x => x.LastName.Substring(0, 1).ToUpper()).ToList(),
            };
        }
    }
}
