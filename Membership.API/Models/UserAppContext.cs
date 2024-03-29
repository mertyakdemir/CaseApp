﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API.Models
{
    public class UserAppContext : IdentityDbContext<User>
    {
        public UserAppContext(DbContextOptions<UserAppContext> options) : base(options)
        {

        }
    }
}
