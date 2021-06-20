using Membership.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.Services
{
    public class GenerateRef
    {
        private readonly UserAppContext _applicationDbContext;

        public GenerateRef(UserAppContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public string GenerateRefCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var randomRefCode = new String(stringChars);

            return randomRefCode;
        }

        public async Task<bool> ValidateRefCode(string refCode)
        {
            var getCodes = await _applicationDbContext.Users.Select(x => x.RefCode).ToListAsync();

            foreach (var item in getCodes)
            {
                if (refCode == item)
                {
                    GenerateRefCode();
                }
            }

            return true;
        }
    }
}
