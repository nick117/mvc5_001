using System.ComponentModel.DataAnnotations;

namespace appraisal.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "工號")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "是否記住工號?")]
        public bool RememberMe { get; set; }
    }
}