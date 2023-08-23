using System.ComponentModel.DataAnnotations;

namespace EasyMeal_WebGUI.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Voornaam is vereist.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Achternaam is vereist.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Straatnaam is vereist.")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "Huisnummer is vereist.")]
        public int HouseNumber { get; set; }
        [Required(ErrorMessage = "Plaatsnaam is vereist.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Postcode is vereist.")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Telefoonnummer is vereist.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Dit is geen geldig telefoonnummer.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is vereist.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
