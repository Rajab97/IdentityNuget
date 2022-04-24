namespace MyBankIdentity.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Username { get; set; }

        public string UserPrincipal { get; set; }

        public string Email { get; set; }

        public string HrCode { get; set; }

        public string BranchCode { get; set; }

        public bool IsEnabled { get; set; }

        public RoleDTO Role { get; set; }
    }
}
