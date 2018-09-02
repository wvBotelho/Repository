using System;
using System.ComponentModel.DataAnnotations;

namespace WVB.Framework.EntityFrameworkRepository.CustomAttributes.ValidationAttributes
{
    /// <summary>
    /// Valida se uma sequência de números contém números repetidos
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class IsRepeatedNumbersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string numerosAValidar = Convert.ToString(value);
                string digito = numerosAValidar.Substring(0, 1);
                int contador = 0;

                for (int i = 1; i < numerosAValidar.Length; i++)
                {
                    if (digito == numerosAValidar.Substring(i, 1))
                        contador++;
                }

                if (contador > numerosAValidar.Length / 2)
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
