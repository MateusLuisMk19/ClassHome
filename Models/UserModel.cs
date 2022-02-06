using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassHome.Models
{
    public class UserModel : IdentityUser<int>
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [NotMapped]
        public int Idade
        {
            get => (int)Math.Floor((DateTime.Now - DataNascimento).TotalDays / 365.25);
        }

        [Display(Name = "Tipo de Utilizador")]
        public string TUsers { get; set; }
        
        ICollection<ProfessorModel> Professores { get; set; }
        ICollection<AlunoModel> Alunos {get; set; }


    }
}