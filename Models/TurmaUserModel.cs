using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class TurmaUserModel
    {
        public int UserId { get; set; }
        
        public int TurmaId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User {get; set;}

        [ForeignKey("TurmaId")]
        public TurmaModel Turma {get; set;}
    }
} 