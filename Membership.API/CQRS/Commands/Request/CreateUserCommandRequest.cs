using MediatR;
using Membership.API.CQRS.Commands.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Membership.API.CQRS.Commands.Request
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonIgnore]
        public string GenerateRef { get; set; }

        [Display(Name = "Reference Code")]
        public string RefCode { get; set; }
        [JsonIgnore]
        public string RefUri { get; set; }
    }
}
