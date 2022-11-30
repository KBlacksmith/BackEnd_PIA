using System.ComponentModel.DataAnnotations;

namespace WebAPICasino.DTOs{
    public class EditarAdminDTO{
        [Required]
        [EmailAddress]
        public string Email {get; set; }
    }
}