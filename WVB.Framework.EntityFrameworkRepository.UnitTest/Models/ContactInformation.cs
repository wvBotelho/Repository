using System.ComponentModel.DataAnnotations;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes.ValidationAttributes;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    public class ContactInformation
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Telefone é obrigatório")]
        [IsRepeatedNumbers(ErrorMessage = "Digite um número de telefone válido")]
        public string Phone { get; set; }
    }
}
