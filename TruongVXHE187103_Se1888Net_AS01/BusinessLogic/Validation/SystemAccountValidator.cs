using System;
using BusinessLogic.DTOs;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class SystemAccountValidator
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        public SystemAccountValidator(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public void ValidateForLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");
        }

        public void ValidateForCreate(SystemAccountDTO accountDto)
        {
            var context = new ValidationContext(accountDto, null, null);
            var results = new List<ValidationResult>();

            // validate tất cả thuộc tính
            Validator.TryValidateObject(accountDto, context, results, true);

            foreach (var result in results)
            {
                foreach (var memberName in result.MemberNames)
                {
                    // ném ArgumentException cho từng lỗi
                    throw new ArgumentException($"{result.ErrorMessage}");
                }
            }

        }
    }
}