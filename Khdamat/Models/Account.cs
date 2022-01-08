using System.ComponentModel.DataAnnotations;

namespace Khdamat.Models
{
    public class Account
    {
        [Key]
        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Password should be between 8 and 20 characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]   
        [Compare("Password", ErrorMessage = "Not the same Password")]
        public string ConfirmPassword { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSupporter { get; set; }
        public bool IsWorker { get; set; }
        public bool IsClient { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Password should be between 8 and 20 characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        public Account()
        {

        }
    }
}
