using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassHome.ViewModels
{
    public class DInnerViewModel
    {
        public int Id { get; set; }

        
        [Display(Name = "Disciplina")]
        public int DisciplinaId { get; set; }

        [Display(Name = "Publicacao")]
        public int PublicacaoId { get; set; }
        public string PublicacaoTexto { get; set; }

        [Display(Name = "Utilizador")]
        public int UserId { get; set; }
    
        [Display(Name = "Comentario")]
        public int ComentarioId { get; set; }
        public string ComentarioTexto { get; set; }

    }
}