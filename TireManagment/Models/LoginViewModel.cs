using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.Models
{
    public class LoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool RememberMe { get; set; }

    }
}
