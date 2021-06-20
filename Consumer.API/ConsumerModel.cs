using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.API
{
    public class ConsumerModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GenerateRef { get; set; }
        public string RefCode { get; set; }
        public string RefUri { get; set; }
    }
}
