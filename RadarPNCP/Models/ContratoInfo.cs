using System;
using System.Collections.Generic;
using System.Text;

namespace RadarPNCP.Models
{
    public class ContratoInfo
    {
        public string? Titulo { get; set; }
        public string? IdPncp { get; set; }
        public string? DataUltimaAtualizacao { get; set; }
        public string? Local { get; set; }
        public string? Orgao { get; set; }
        public string? UnidadeCompradora { get; set; }
        public string? Modalidade { get; set; }
        public string? AmparoLegal { get; set; }
        public string? Tipo { get; set; }
        public string? ModoDisputa { get; set; }
        public string? RegistroPreco { get; set; }
        public string? FonteOrcamentaria { get; set; }
        public string? DataDivulgacaoPncp { get; set; }
        public string? Situacao { get; set; }
        public string? DataInicioPropostas { get; set; }
        public string? DataFimPropostas { get; set; }
        public string? ValorTotalEstimado { get; set; }
        public string? Objeto { get; set; }
        public string? FonteSistema { get; set; }

        public List<ItemContratacao> Itens { get; set; } = new();
        public List<ArquivoContratacao> Arquivos { get; set; } = new();
        public List<HistoricoContratacao> Historico { get; set; } = new();
    }
}
