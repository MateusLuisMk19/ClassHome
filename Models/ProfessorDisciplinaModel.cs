using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class ProfessorDisciplinaModel
    {
        public int ProfessorId { get; set; }
        
        public int DisciplinaId { get; set; }

        [ForeignKey("ProfessorId")]
        public ProfessorModel Professer {get; set;}

        [ForeignKey("DisciplinaId")]
        public DisciplinaModel Disciplina {get; set;}
    }
}