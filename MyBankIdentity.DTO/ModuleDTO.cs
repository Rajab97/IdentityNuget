using System;
using System.Collections.Generic;
using System.Text;

namespace MyBankIdentity.DTO
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
