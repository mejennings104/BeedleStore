using Microsoft.EntityFrameworkCore; //Added to address error with primary key with cvm
using System.ComponentModel.DataAnnotations;

namespace GadgetStore.UI.MVC.Models
{
    [Keyless]
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Required")]
        [EmailAddress(ErrorMessage = "*Must be a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Required")]
        [DataType(DataType.MultilineText)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Message { get; set; }
    }
}
