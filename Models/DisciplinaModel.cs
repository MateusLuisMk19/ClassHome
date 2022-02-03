
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassHome
{
    public class DisciplinaModel
    {
        [Key]
        public int DisciplinaId { get; set;}

        [Required]
        [MaxLength(50)]
        public string Nome { get; set;}
        
        [MaxLength(250)]
        public string Descricao { get; set;}
        public int TurmaId { get; set;}

        [ForeignKey("TurmaId")]
        public TurmaModel Turma { get; set;}
        ICollection<MatriculaModel> Matricula { get; set; }
        ICollection<ProfessorDisciplinaModel> ProfessorDisciplina { get; set; }
        ICollection<Publicacao> Publicacoes { get; set; }

    }

}