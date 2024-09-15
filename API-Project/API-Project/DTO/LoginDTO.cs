using System.ComponentModel;

namespace API_Project.DTO
{
    public class LoginDTO
    {
        [DisplayName("Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
