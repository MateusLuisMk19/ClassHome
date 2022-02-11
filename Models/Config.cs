using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassHome.Models;

namespace ClassHome
{
    public class Config
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fundo { get; set; }
        public string Texto { get; set; }
        public string CardStyle { get; set; }
        public string CardStyleCab { get; set; }
        public Estado Estado { get; set; }
        public string Menu { get; set; }
        public string Tabela { get; set; }
        public string TabelaText { get; set; }
        public string BTNin { get; set; }
        public string BTNoff { get; set; }
        public string Link { get; set; }
        public string CardCab { get; set; }
        public string TitleText { get; set; }
        public string DCPBannerStyle { get; set; }
        public string DCPBoxColor { get; set; }
        public string DCPBoxText { get; set; }

    }
    public enum Estado
    {
        Desativado,Ativado
    }

}