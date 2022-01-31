using System.ComponentModel.DataAnnotations;

namespace ClassHome.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Usuário")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string User { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Senha { get; set; }

        [Required]
        [Display(Name = "Lembrar de mim")]
        public bool Lembrar { get; set; }

        public string ReturnUrl { get; set; }
    }
}