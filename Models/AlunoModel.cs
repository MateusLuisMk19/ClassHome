using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class AlunoModel
    {
        [Key]
        public int AlunoId { get; set; }
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        ICollection<MatriculaModel> Matriculas {get; set;}
    }
}