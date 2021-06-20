using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Commands.Response
{
    public class CreateUserCommandResponse
    {
        public bool IsSuccess { get; set; }
    }
}
