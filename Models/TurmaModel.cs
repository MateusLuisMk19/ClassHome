
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassHome
{
    public class TurmaModel
    {
        [Key]
        public int TurmaId { get; set;}

        public string NomeCurso { get; set;}

        public string Local {get; set;}

        public string Descricao { get; set;}

        public int CriadorId { get; set;}
        ICollection<DisciplinaModel> Disciplinas { get; set; }
        ICollection<TurmaUserModel> Professores { get; set; } 


    }
}