namespace MyBankIdentity.Middleware
{
    public class JwtMiddlewareOptions
    {
        public string Secret { get; set; }

        public string ModuleName { get; set; }
    }
}
