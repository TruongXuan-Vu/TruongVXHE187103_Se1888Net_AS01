using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class DbInitialLizer
    {
        public static async Task SeedAdminAccountAsync(ApplicationDbContext context, IConfiguration configuration)
        {
            var adminEmail = configuration["AdminAccount:Email"];
            var adminPassword = configuration["AdminAccount:Password"];

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
                throw new Exception("Admin account configuration missing in appsettings.json");

            var existingAdmin = await context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == adminEmail);

            if (existingAdmin == null)
            {
                var admin = new SystemAccount
                {
                    AccountName = "Admin",
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole = 0 // 0 = Admin
                };

                await context.SystemAccounts.AddAsync(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
