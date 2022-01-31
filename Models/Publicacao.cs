
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassHome
{
    public class Publicacao
    {
        [Key]
        public int PublicacaoId { get; set;}

        public string Texto { get; set;}

        public DateTime? DataPublicacao {get;}

        public int AlunoId { get; set;}

        [ForeignKey("AlunoId")]
        public AlunoModel Aluno { get; set; }

        public int DisciplinaId { get; set;}

        [ForeignKey("DisciplinaId")]
        public DisciplinaModel Disciplina { get; set; }

        ICollection<Comentario> Comentarios { get; set; }


    }

    
}