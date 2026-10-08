using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class Socio
    {
        public int SocioId { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
        [DataType(DataType.Text)]
        [Display(Name = "DNI")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        public bool Activo { get; set; }
    }
}