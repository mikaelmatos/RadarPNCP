using Microsoft.Web.WebView2.Core;
using RadarLicitacoes.Util;
using RadarPNCP.Htmls;
using RadarPNCP.Models;
using RadarPNCP.Repository;
using RadarPNCP.Util;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RadarPNCP
{
    public partial class Form1 : Form
    {
        int novas = 0;
        int jaExtistia = 0;
        string promptBase = "";
        List<LicitacaoResumo> licitacoes = new List<LicitacaoResumo>();

        public Form1()
        {
            InitializeComponent();

            webView22.CoreWebView2InitializationCompleted += WebView22_Initialized;
            webView22.EnsureCoreWebView2Async(null);
        }
        private async void WebView22_Initialized(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                webView22.WebMessageReceived += WebView22_MessageReceived;

                licitacoes = await LicitacaoResumoRepository.ListarAsync();
                webView22.NavigateToString(Base.HomeDarkLight(licitacoes));
            }
        }
        private void WebView22_MessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string mensagem = e.TryGetWebMessageAsString();

            switch (mensagem)
            {
                case "salvar":
                    //SalvarRegistro();
                    break;
                case "novo":
                    //NovoRegistro();
                    break;
                case "site":
                    Web.AbrirLink("https://roquesystems.com/");
                    break;
            }
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

            webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;

            List<LicitacaoResumo> licitacoesPrincipal = new List<LicitacaoResumo>();

            progressBar1.Maximum = termos.Split("\n").ToList().Count;

            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            foreach (string termo in termos.Split("\n").ToList())
            {
                licitacoesPrincipal.AddRange(await Extrair(termo, 2));

                progressBar1.Value = progressBar1.Value + 1;
                labelProgresso.Text = progressBar1.Value + "/" + progressBar1.Maximum + " (" + novas + " novo" + (novas > 1 ? "s" : "") + " registro" + (novas > 1 ? "s" : "") + " - " + +jaExtistia + " registro" + (jaExtistia > 1 ? "s" : "") + " já existia" + (jaExtistia > 1 ? "m" : "") + ") " + stopwatch.Elapsed.ToString().Split('.')[0];
            }

            stopwatch.Stop();
            MessageBox.Show("Processo finalizado! Novas: " + novas + " Já existiam: " + jaExtistia + "(" + novas + " novo " + (novas > 1 ? "s" : "") + " registro" + (novas > 1 ? "s" : "") + ") - Tempo gasto: " + stopwatch.Elapsed);
        }

        private void WebView21_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
                document.addEventListener('contextmenu', e => e.preventDefault());
                document.addEventListener('keydown', e => {
                    const key = e.key.toUpperCase();
                    const ctrl = e.ctrlKey || e.metaKey;
                    const bloqueados = [
                        ctrl && key === 'R', ctrl && key === 'F', ctrl && key === 'U',
                        ctrl && key === 'S', ctrl && key === 'P', ctrl && key === 'D',
                        ctrl && e.shiftKey && key === 'I', ctrl && e.shiftKey && key === 'J',
                        e.key === 'F5', e.key === 'F12'
                    ];
                    if (bloqueados.some(Boolean)) e.preventDefault();
                });
                document.addEventListener('dragstart', e => e.preventDefault());
            ");
            webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"(function () {
                document.addEventListener('contextmenu', e => e.preventDefault());
                document.addEventListener('keydown', e => {
                const key = e.key.toUpperCase();
                const ctrl = e.ctrlKey || e.metaKey;
                const bloqueados = [
                    ctrl && key === 'R',          // Ctrl+R — recarregar
                    ctrl && key === 'F',          // Ctrl+F — localizar
                    ctrl && key === 'U',          // Ctrl+U — ver fonte
                    ctrl && key === 'S',          // Ctrl+S — salvar página
                    ctrl && key === 'P',          // Ctrl+P — imprimir
                    ctrl && key === 'D',          // Ctrl+D — favoritos
                    ctrl && key === 'N',          // Ctrl+N — nova janela
                    ctrl && key === 'T',          // Ctrl+T — nova aba
                    ctrl && key === 'W',          // Ctrl+W — fechar aba
                    ctrl && key === 'H',          // Ctrl+H — histórico
                    ctrl && key === 'J',          // Ctrl+J — downloads
                    ctrl && e.shiftKey && key === 'I',  // Ctrl+Shift+I — DevTools
                    ctrl && e.shiftKey && key === 'J',  // Ctrl+Shift+J — Console
                    ctrl && e.shiftKey && key === 'C',  // Ctrl+Shift+C — Inspecionar elemento
                    e.key === 'F5',               // F5 — recarregar
                    e.key === 'F12',              // F12 — DevTools
                ];
                if (bloqueados.some(Boolean)) e.preventDefault();
                });
                document.addEventListener('dragstart', e => e.preventDefault());
                document.addEventListener('selectstart', e => e.preventDefault());
            })();");
            webView21.CoreWebView2.Settings.IsZoomControlEnabled = false;


        }

        public async Task<List<LicitacaoResumo>> Extrair(string termo, int paginasQnt)
        {
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
                            double nota = await GroqAvaliador.AvaliarTextoAsync(promptBase, licitacaoResumo.Objeto);
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
                            await webView22.CoreWebView2.ExecuteScriptAsync(Base.AtualizarDados(licitacoes));

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

        private void buttonAbrirSite_Click(object sender, EventArgs e)
        {
            //Mantenha isso no projeto :)
            Web.AbrirLink("https://roquesystems.com/");
        }


    }
}
