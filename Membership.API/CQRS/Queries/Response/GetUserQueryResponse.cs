using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Queries.Response
{
    public class GetUserQueryResponse
    {
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
