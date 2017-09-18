using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthNoneEf.Models
{
    public class AuthRole : AppModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NormalizedRoleName { get; set; }

        public string RoleId => this.Id.ToString();
    }
}
