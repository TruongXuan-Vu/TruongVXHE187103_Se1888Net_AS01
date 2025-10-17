using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class SystemAccountDTO
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(2, ErrorMessage = "Password must be at least 2 characters!")]
        public string? AccountPassword { get; set; }
    }
}