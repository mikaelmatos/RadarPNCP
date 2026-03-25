using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RadarPNCP.Models
{
    public class LicitacaoResumo
    {
        public string UrlDetalhes { get; set; }
        public string IdPNCP { get; set; }
        public string Modalidade { get; set; }
        public string Orgao { get; set; }
        public string Objeto { get; set; }
        public string Local { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public double Nota { get; set; }
    }
}
