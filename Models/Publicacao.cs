
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class Publicacao
    {
        [Key]
        public int PublicacaoId { get; set;}

        public string Texto { get; set;}

        public DateTime? DataPublicacao {get;}

        public int UserId { get; set;}

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        public int DisciplinaId { get; set;}

        [ForeignKey("DisciplinaId")]
        public DisciplinaModel Disciplina { get; set; }

        ICollection<Comentario> Comentarios { get; set; }


    } 
}