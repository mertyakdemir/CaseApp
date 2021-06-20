using MediatR;
using Membership.API.CQRS.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Queries.Request
{
    public class GetUserQueryRequest : IRequest<GetUserQueryResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
