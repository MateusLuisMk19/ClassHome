
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
public class Comentario
    {  
        [Key]
        public int ComentarioId { get; set;}

        public string Texto { get; set;}

        public DateTime DataComentario {get;}
        public int UserId { get; set;}

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        public int PublicacaoId { get; set;}

        [ForeignKey("PublicacaoId")]
        public Publicacao Publicacao { get; set; }
    }
}