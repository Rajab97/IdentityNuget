using System.Collections.Generic;

namespace MyBankIdentity.DTO
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
