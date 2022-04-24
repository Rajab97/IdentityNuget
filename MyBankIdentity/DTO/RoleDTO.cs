using System.Collections.Generic;

namespace MyBankIdentity.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ModuleDTO> Modules { get; set; }
    }
}
