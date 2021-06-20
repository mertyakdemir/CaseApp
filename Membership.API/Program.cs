using Membership.API.CQRS.Commands.Request;
using Membership.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Membership.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var appDbContext = serviceProvider.GetRequiredService<UserAppContext>();

                appDbContext.Database.Migrate();

                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                string uri = "http://localhost:25244/user/signup/";

                if (!userManager.Users.Any())
                {
                    userManager.CreateAsync(new User { FirstName = "First Name", LastName = "Last Name", Email = "test@gmail.com", UserName = "test@gmail.com", GenerateRef = "111", RefCode = "333", RefUri = uri + "111" }, "testpass").Wait();

                    userManager.CreateAsync(new User { FirstName = "First Name 2", LastName = "Last Name 2", Email = "test2@gmail.com", UserName = "test2@gmail.com", GenerateRef = "222", RefCode = "333", RefUri = uri + "222" }, "testpass2").Wait();

                    userManager.CreateAsync(new User { FirstName = "First Name 3", LastName = "Last Name 3", Email = "test3@gmail.com", UserName = "test3@gmail.com", GenerateRef = "333", RefCode = "222", RefUri = uri + "333" }, "testpass3").Wait();

                    userManager.CreateAsync(new User { FirstName = "First Name 4", LastName = "Last Name 4", Email = "test4@gmail.com", UserName = "test4@gmail.com", GenerateRef = "444", RefCode = "222", RefUri = uri + "444" }, "testpass4").Wait();

                    userManager.CreateAsync(new User { FirstName = "First Name 5", LastName = "Last Name 5", Email = "test5@gmail.com", UserName = "test5@gmail.com", GenerateRef = "555", RefCode = "111", RefUri = uri + "555" }, "testpass5").Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
