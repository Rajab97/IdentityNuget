using Microsoft.AspNetCore.Mvc;
using MyBankIdentity.DTO;
using Newtonsoft.Json;

namespace MyBankIdentity.Controllers
{
    public class MyController : ControllerBase
    {
        public UserDTO CurrentUser
        {
            get
            {
                return JsonConvert.DeserializeObject<UserDTO>(HttpContext.Items["User"].ToString());
            }
            set { }
        }

    }
}
