/* using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class ProfessorModel
    {
        [Key]
        public int ProfessorId { get; set; }
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        ICollection<ProfessorDisciplinaModel> Disciplinas {get; set; } 
        ICollection<TurmaUserModel> Turmas {get; set; } 
    }
} */