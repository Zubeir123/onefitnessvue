using System.ComponentModel.DataAnnotations;

namespace FitnessTimeGym.ViewModel.Login
{
    public class ForgotPasswordViewModel
    {
        [StringLength(30, ErrorMessage = "Not valid Username")]
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }
    }
}