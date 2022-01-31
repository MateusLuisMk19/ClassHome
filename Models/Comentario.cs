
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassHome
{
public class Comentario
    {  
        [Key]
        public int ComentarioId { get; set;}

        public string Texto { get; set;}

        public int AlunoId { get; set;}

        [ForeignKey("AlunoId")]
        public AlunoModel Aluno { get; set; }

        public int PublicacaoId { get; set;}

        [ForeignKey("PublicacaoId")]
        public Publicacao Publicacao { get; set; }
    }
}