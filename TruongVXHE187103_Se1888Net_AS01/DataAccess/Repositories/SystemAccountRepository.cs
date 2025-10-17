
using DataAccess.Context;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemAccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAccountAsync(SystemAccount account)
        {
            await _dbContext.SystemAccounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            await _dbContext.SystemAccounts.Where(c => c.AccountId == accountId).ExecuteDeleteAsync();
        }

        public async Task<SystemAccount> GetAccountAsync(string accountEmail)
        {
            return await _dbContext.SystemAccounts.FirstOrDefaultAsync(c => c.AccountEmail == accountEmail);
        }

        public async Task<SystemAccount> GetAccountByIdAsync(int accountId)
        {
            return await _dbContext.SystemAccounts.FirstOrDefaultAsync(c => c.AccountId == accountId);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountAsync(string? accountNameOrEmail, int? accountRole)
        {
            var query = _dbContext.SystemAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(accountNameOrEmail))
            {
                query = query.Where(a => a.AccountName.Contains(accountNameOrEmail) || a.AccountEmail.Contains(accountNameOrEmail));
            }

            if (accountRole.HasValue)
            {
                query = query.Where(a => a.AccountRole == accountRole.Value);
            }

            return await query.ToListAsync();
        }

        public Task UpdateAccountAsync(SystemAccount account)
        {
            _dbContext.SystemAccounts.Update(account);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetLastIdAsync()
        {
            return await _dbContext.SystemAccounts
                .OrderByDescending(a => a.AccountId)
                .Select(a => a.AccountId)
                .FirstOrDefaultAsync();
        }
    }
}