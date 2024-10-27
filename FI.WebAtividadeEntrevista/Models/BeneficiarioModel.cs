using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [RegularExpression(@"^([0-9]{3})\.([0-9]{3})\.([0-9]{3})-([0-9]{2})$",
         ErrorMessage = "Digite um CPF no formato 000.000.000-00!")]
        [Cpf(ErrorMessage = "Digite um CPF válido!")]
        public string CPF { get; set; }

        public bool Novo { get; set; }

    }
}