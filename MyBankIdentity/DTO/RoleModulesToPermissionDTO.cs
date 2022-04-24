using Newtonsoft.Json;

namespace MyBankIdentity.DTO
{
    public class RoleModulesToPermissionDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int? PermissionId { get; set; }
    }
}
