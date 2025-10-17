using BusinessLogic.DTOs;
using BusinessLogic.Validation;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class SystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly IConfiguration _configuration;
        private readonly SystemAccountValidator _accountValidator;

        public SystemAccountService(ISystemAccountRepository systemAccountRepository, IConfiguration configuration, SystemAccountValidator accountValidator)
        {
            _systemAccountRepository = systemAccountRepository;
            _configuration = configuration;
            _accountValidator = accountValidator;
        }

        public async Task<SystemAccount> LoginAsync(string email, string password)
        {
            _accountValidator.ValidateForLogin(email, password);

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            if (email == adminEmail)
            {
                var admin = new SystemAccount
                {
                    AccountId = short.Parse(_configuration["AdminAccount:Id"]),
                    AccountEmail = adminEmail,
                    AccountName = _configuration["AdminAccount:Name"],
                    AccountPassword = adminPassword,
                    AccountRole = int.Parse(_configuration["AdminAccount:Role"])
                };

                if (admin.AccountPassword != password) throw new UnauthorizedAccessException("Password is incorrect!");

                return admin;
            }

            var account = await _systemAccountRepository.GetAccountAsync(email);

            if (account == null) throw new InvalidOperationException("Account not found!");
            if (account.AccountPassword != password) throw new UnauthorizedAccessException("Password is incorrect!");

            return account;
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountAsync(string? accountNameOrEmail, int? accountRole) => await _systemAccountRepository.SearchAccountAsync(accountNameOrEmail, accountRole);

        public async Task AddAccountAsync(SystemAccountDTO accountDto)
        {
            _accountValidator.ValidateForCreate(accountDto);

            var lastId = await _systemAccountRepository.GetLastIdAsync();

            var account = new SystemAccount
            {
                AccountId = (short)(lastId + 1),
                AccountName = accountDto.AccountName,
                AccountEmail = accountDto.AccountEmail,
                AccountPassword = accountDto.AccountPassword,
                AccountRole = accountDto.AccountRole
            };

            var existingAccount = await _systemAccountRepository.GetAccountAsync(account.AccountEmail);
            if (existingAccount != null) throw new InvalidOperationException("Account already exists!");

            await _systemAccountRepository.AddAccountAsync(account);
        }

        public async Task<SystemAccount> GetAccountByIdAsync(int accountId) => await _systemAccountRepository.GetAccountByIdAsync(accountId);


    }
}
