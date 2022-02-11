using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class MatriculaModel
    {
        [Key]
        public int MatriculaId { get; set; }
        
        public int AlunoId { get; set; }
    
        [ForeignKey("AlunoId")]
        public UserModel Aluno {get; set;}
        public int TurmaId { get; set; }
        public int DisciplinaId { get; set; }

        [ForeignKey("DisciplinaId")]
        public DisciplinaModel Disciplina {get; set;}
    }
}