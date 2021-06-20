using Membership.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.Services
{
    public class ApplyRefCode
    {
        private readonly UserAppContext _applicationDbContext;
        public ApplyRefCode(UserAppContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string AssignRefCode(string refCode)
        {
            var code = _applicationDbContext.Users.Where(i => i.GenerateRef == refCode).FirstOrDefault();

            if (code == null)
            {
                return "";
            }

            return code.GenerateRef;
        }
    }
}
