using System.ComponentModel.DataAnnotations;

namespace UserService.JwtToken
{
    public class UserValidationRequestModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
