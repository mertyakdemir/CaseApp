using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Queries.Response
{
    public class GetByIdUserQueryResponse
    {
        public List<string> FirstName { get; set; }
        public List<string> LastName { get; set; }
        public string RefUri { get; set; }
        public string RefCode { get; set; }
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }
}
