using System.ComponentModel.DataAnnotations;

namespace WebApp7.ViewModels
{
    public class LoginVm
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
