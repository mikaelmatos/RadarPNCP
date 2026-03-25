using Microsoft.Web.WebView2.Core;
using RadarLicitacoes.Util;
using RadarPNCP.Models;
using RadarPNCP.Repository;
using RadarPNCP.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RadarPNCP
{
    public partial class Form1 : Form
    {
        int novas = 0;
        int jaExtistia = 0;
        string promptBase = "";

        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            var env = await CoreWebView2Environment.CreateAsync(null, @"data-cookies");
            await webView21.EnsureCoreWebView2Async(env);

            StreamReader readerPromptBase = new StreamReader("SKILL-COMPATIBILIDADE.md");
            promptBase = readerPromptBase.ReadToEnd().Replace("\r", "");
            readerPromptBase.Close();

            StreamReader reader = new StreamReader("lista-termos.txt");
            string termos = reader.ReadToEnd().Replace("\r", "");
            reader.Close();

            List<LicitacaoResumo> licitacoesPrincipal = new List<LicitacaoResumo>();

            foreach (string termo in termos.Split("\n").ToList())
            {
                licitacoesPrincipal.AddRange(await Extrair(termo, 2));
            }

            MessageBox.Show("Processo finalizado! Novas: " + novas + " Já existiam: " + jaExtistia);
        }

        public async Task<List<LicitacaoResumo>> Extrair(string termo, int paginasQnt)
        {
            List<LicitacaoResumo> licitacoes = new List<LicitacaoResumo>();

            for (int pagina = 0; pagina < paginasQnt; pagina++)
            {
                webView21.CoreWebView2.Navigate("https://pncp.gov.br/app/editais?q=" + termo + "&status=recebendo_proposta&pagina=" + pagina + "&tipos=3%7C1%7C4&tam_pagina=100");
                await Task.Delay(5000);
                string listaBruta = await webView21.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName(\"br-list\")[1].innerHTML");
                string listaLimpa = Textos.UnificaSimbolosRepetidos("¢", Textos.RemoverTags(listaBruta.Replace("\\u003C", "<").Replace("br-item", "\n\r").Replace(">", ">¢").Replace("\\\" href=\\\"", "").Replace("\\\">", "")));

                foreach (string str in listaLimpa.Split("\n").ToList())
                {
                    if (str.Contains("editais/"))
                    {
                        string str_corrigida = str.Split('<')[0];

                        LicitacaoResumo licitacaoResumo = new LicitacaoResumo()
                        {
                            UrlDetalhes = "https://pncp.gov.br/app" + str_corrigida.Split('¢')[0].Trim(),
                            IdPNCP = str_corrigida.Split('¢')[1].Trim(),
                            Modalidade = str_corrigida.Split('¢')[2].Trim(),
                            Orgao = str_corrigida.Split('¢')[4].Trim(),
                            Objeto = str_corrigida.Split('¢')[6].Trim(),
                            Local = str_corrigida.Split('¢')[5].Trim(),
                            UltimaAtualizacao = DateTime.Parse(str_corrigida.Split('¢')[3].Trim())                         
                        };

                        if (!await LicitacaoResumoRepository.ExistePorIdPncpAsync(licitacaoResumo.IdPNCP))
                        {
                            double nota = await GroqAvaliador.AvaliarTextoAsync(promptBase,licitacaoResumo.Objeto);
                            licitacaoResumo.Nota = nota;

                            int tentativas = 0;

                            while (nota == -1 || tentativas > 5)
                            {
                                nota = await GroqAvaliador.AvaliarTextoAsync(promptBase, licitacaoResumo.Objeto);
                                licitacaoResumo.Nota = nota;
                                tentativas++;
                            }

                            if (nota == -1 || tentativas > 5)
                            {
                                Application.Restart();
                            }

                            await LicitacaoResumoRepository.InserirSeNaoExistirAsync(licitacaoResumo);

                            licitacoes.Add(licitacaoResumo);
                            novas++;
                        }
                        else
                        {
                            jaExtistia++;
                        }
                    }
                }
            }

            return licitacoes;
        }
    }
}
