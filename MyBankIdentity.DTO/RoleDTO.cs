using System;
using System.Collections.Generic;
using System.Text;

namespace MyBankIdentity.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ModuleDTO> Modules { get; set; }
    }
}
