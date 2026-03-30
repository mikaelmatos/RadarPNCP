using RadarPNCP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RadarPNCP.Htmls
{
    public class Base
    {
        public static string AtualizarDados(List<LicitacaoResumo> licitacoesResumo)
        {
            if (licitacoesResumo == null)
                licitacoesResumo = new List<LicitacaoResumo>();

            string json = JsonSerializer.Serialize(licitacoesResumo);

            return $@"
                (function() {{
                    DB.length = 0;
                    const novos = {json};
                    novos.forEach(r => DB.push(r));
                    filtrar();
                }})();
            ";
        }
        public static string HomeDarkLight(List<LicitacaoResumo> licitacoesResumo = null)
        {
            if (licitacoesResumo == null)
            {
                licitacoesResumo = new List<LicitacaoResumo>();
            }

            string jsonLicitacoesResumo = JsonSerializer.Serialize(licitacoesResumo);

            string resposta =
                @"
                <!DOCTYPE html>
                <html lang=""pt-BR"">
                <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Radar PNCP — Roque Systems</title>
                <style>
                @import url('https://fonts.googleapis.com/css2?family=Exo+2:wght@300;400;500;600&display=swap');

                *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

                /* ── BLOQUEIOS GLOBAIS ── */
                html {
                  -webkit-user-select: none;
                  user-select: none;
                  touch-action: none;
                }
                /* re-habilita seleção apenas nos inputs e textareas */
                input, textarea, select {
                  -webkit-user-select: text;
                  user-select: text;
                }

                :root { --radius: 10px; --radius-sm: 6px; --tr: .18s ease; }

                [data-theme=""dark""] {
                  --bg:         #07111f;
                  --surface:    #0b1c30;
                  --panel:      #0e2440;
                  --border:     #1a4a72;
                  --cyan:       #00c8e8;
                  --cyan2:      #00a0c0;
                  --accent:     #00e5ff;
                  --text:       #cce8f8;
                  --text-dim:   #6a9ab8;
                  --row-even:   rgba(0,140,200,.06);
                  --row-hover:  rgba(0,200,232,.13);
                  --hdr:        linear-gradient(90deg,rgba(21,101,192,.55),rgba(0,140,180,.3));
                  --hdr-bg:     #0d2a45;
                  --tab-bg:     #0b1c30;
                  --tab-act:    #0e2440;
                  --shadow:     0 0 18px rgba(0,120,180,.1);
                  --prog:       linear-gradient(90deg,#1976d2,#00c8e8);
                  --link:       #00c8e8;
                  --nh:         #00d48a; --nm: #ffcc44; --nl: #ff6b6b;
                  --bpe-bg:rgba(0,180,120,.18);  --bpe-c:#00d48a; --bpe-b:rgba(0,180,120,.3);
                  --bpp-bg:rgba(255,180,0,.15);  --bpp-c:#ffcc44; --bpp-b:rgba(255,180,0,.25);
                  --bco-bg:rgba(220,50,50,.15);  --bco-c:#ff6b6b; --bco-b:rgba(220,50,50,.25);
                  --knob: #00c8e8; --trk: #0b1c30;
                  --sb: #0b1c30;
                }

                [data-theme=""light""] {
                  --bg:         #eef3f8;
                  --surface:    #ffffff;
                  --panel:      #ffffff;
                  --border:     #c5d8ec;
                  --cyan:       #005f8a;
                  --cyan2:      #00799e;
                  --accent:     #0095c8;
                  --text:       #1a2e42;
                  --text-dim:   #5a7a96;
                  --row-even:   rgba(0,100,160,.04);
                  --row-hover:  rgba(0,140,200,.09);
                  --hdr:        linear-gradient(90deg,rgba(0,100,160,.1),rgba(0,130,170,.05));
                  --hdr-bg:     #dde8f2;
                  --tab-bg:     #dde8f2;
                  --tab-act:    #ffffff;
                  --shadow:     0 2px 12px rgba(0,80,140,.08);
                  --prog:       linear-gradient(90deg,#0077a8,#00bcd4);
                  --link:       #0077a8;
                  --nh:         #007a50; --nm: #8a6000; --nl: #a02020;
                  --bpe-bg:rgba(0,140,90,.1);   --bpe-c:#007a50; --bpe-b:rgba(0,140,90,.2);
                  --bpp-bg:rgba(180,120,0,.1);  --bpp-c:#8a6000; --bpp-b:rgba(180,120,0,.2);
                  --bco-bg:rgba(180,40,40,.1);  --bco-c:#a02020; --bco-b:rgba(180,40,40,.2);
                  --knob: #005f8a; --trk: #ccdde8;
                  --sb: #f5f8fb;
                }

                html, body {
                  width: 100%;
                  height: 100%;
                  min-width: 480px;
                  overflow: hidden;
                  background: var(--bg);
                  font-family: 'Exo 2', sans-serif;
                  font-size: 12px;
                  color: var(--text);
                  transition: background var(--tr), color var(--tr);
                }

                #app {
                  display: flex; flex-direction: column;
                  width: 100%; height: 100%;
                  padding: 7px 8px 0; gap: 5px;
                }

                /* TOPBAR */
                #topbar {
                  display: flex; align-items: center; gap: 10px;
                  height: 34px; flex-shrink: 0; padding: 0 2px;
                }

                #logo-wrap {
                  cursor: pointer; display: flex; align-items: center;
                  height: 30px; flex-shrink: 0;
                  transition: opacity .15s;
                }
                #logo-wrap:hover { opacity: .75; }
                #logo-wrap img { height: 26px; width: auto; display: block; }
                #logo-fallback {
                  display: none; font-size: 13px; font-weight: 700;
                  color: var(--cyan); letter-spacing: .06em;
                }

                .topbar-title {
                  font-size: 14px; font-weight: 600;
                  letter-spacing: .06em; color: var(--cyan);
                  text-transform: uppercase; flex: 1;
                  white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
                }
                .topbar-title span {
                  font-weight: 300; color: var(--text-dim);
                  font-size: 10.5px; letter-spacing: .04em;
                  margin-left: 6px; text-transform: none;
                }

                .theme-toggle {
                  display: flex; align-items: center; gap: 6px;
                  font-size: 11px; color: var(--text-dim);
                  cursor: pointer; user-select: none; flex-shrink: 0;
                }
                .toggle-track {
                  width: 34px; height: 18px;
                  background: var(--trk); border: 1px solid var(--border);
                  border-radius: 9px; position: relative;
                  transition: background var(--tr), border-color var(--tr); flex-shrink: 0;
                }
                .toggle-knob {
                  width: 12px; height: 12px;
                  background: var(--knob); border-radius: 50%;
                  position: absolute; top: 2px; left: 2px;
                  transition: transform .2s, background var(--tr);
                }
                .toggle-knob.r { transform: translateX(16px); }

                /* DIVIDER */
                .divider {
                  height: 1px; flex-shrink: 0; margin: 0 2px;
                  background: linear-gradient(90deg,transparent,var(--border) 15%,var(--cyan2) 50%,var(--border) 85%,transparent);
                }

                /* FILTROS */
                #filters {
                  display: flex; gap: 6px; align-items: center;
                  flex-shrink: 0; min-height: 27px;
                  padding: 0 2px; flex-wrap: wrap;
                }
                .fl { font-size: 10px; color: var(--text-dim); text-transform: uppercase; letter-spacing: .05em; white-space: nowrap; }

                .fi {
                  height: 25px;
                  background: var(--panel); border: 1px solid var(--border);
                  border-radius: var(--radius-sm); color: var(--text);
                  font-family: 'Exo 2', sans-serif; font-size: 11px;
                  padding: 0 8px; outline: none;
                  transition: border-color var(--tr), box-shadow var(--tr);
                }
                .fi:focus { border-color: var(--cyan); box-shadow: 0 0 0 2px rgba(0,180,220,.12); }
                .fi.wide { flex: 1; min-width: 120px; }
                .fi.sm   { width: 120px; }
                .fi.xs   { width: 48px; }

                .btn {
                  height: 25px; padding: 0 11px;
                  border-radius: var(--radius-sm); border: 1px solid var(--border);
                  font-family: 'Exo 2', sans-serif; font-size: 11px; font-weight: 600;
                  letter-spacing: .04em; cursor: pointer; transition: all var(--tr); white-space: nowrap;
                }
                .btn-p {
                  background: linear-gradient(135deg,#1565c0,#0097b2);
                  color: #e0f7ff; border-color: var(--cyan2);
                  box-shadow: 0 2px 6px rgba(0,140,190,.2);
                }
                .btn-p:hover { background: linear-gradient(135deg,#1976d2,#00bcd4); border-color: var(--accent); }
                .btn-g { background: var(--panel); color: var(--text-dim); }
                .btn-g:hover { color: var(--cyan); border-color: var(--cyan2); }

                /* MAIN / ABAS */
                #main { flex: 1; display: flex; flex-direction: column; min-height: 0; }

                .tabs-bar { display: flex; gap: 3px; padding: 0 2px; align-items: flex-end; flex-shrink: 0; flex-wrap: wrap; }

                .tab {
                  height: 27px; padding: 0 13px;
                  border-radius: var(--radius) var(--radius) 0 0;
                  border: 1px solid var(--border); border-bottom: none;
                  background: var(--tab-bg); color: var(--text-dim);
                  font-family: 'Exo 2', sans-serif; font-size: 10.5px;
                  font-weight: 500; letter-spacing: .05em; text-transform: uppercase;
                  cursor: pointer; transition: background var(--tr), color var(--tr);
                  display: flex; align-items: center; gap: 5px;
                  position: relative; bottom: -1px;
                }
                .tab.active {
                  background: var(--tab-act); color: var(--cyan);
                  border-bottom: 1px solid var(--tab-act); z-index: 1;
                }
                .tab:not(.active):hover { color: var(--text); }
                .tdot { width: 5px; height: 5px; border-radius: 50%; background: currentColor; opacity: .5; }

                .tabs-content {
                  flex: 1; background: var(--panel); border: 1px solid var(--border);
                  border-radius: 0 var(--radius) var(--radius) var(--radius);
                  overflow: hidden; display: flex; flex-direction: column;
                  box-shadow: var(--shadow); transition: background var(--tr), border-color var(--tr);
                }

                .tab-pane { display: none; flex: 1; flex-direction: column; overflow: hidden; }
                .tab-pane.active { display: flex; }

                /* TABELA */
                .tw { flex: 1; overflow-y: auto; overflow-x: hidden; }
                .tw::-webkit-scrollbar { width: 5px; }
                .tw::-webkit-scrollbar-track { background: transparent; }
                .tw::-webkit-scrollbar-thumb { background: var(--border); border-radius: 4px; }
                .tw::-webkit-scrollbar-thumb:hover { background: var(--cyan2); }

                table { width: 100%; border-collapse: collapse; font-size: 11px; table-layout: fixed; }

                /* FIX: cabeçalho opaco para não misturar com conteúdo no scroll */
                thead tr {
                  background-color: var(--hdr-bg);
                  background-image: var(--hdr);
                  position: sticky; top: 0; z-index: 2;
                  box-shadow: 0 1px 0 var(--border);
                }

                th {
                  padding: 5px 8px; text-align: left; font-weight: 600;
                  letter-spacing: .05em; text-transform: uppercase;
                  font-size: 9.5px; color: var(--cyan);
                  border-bottom: 1px solid var(--border); white-space: nowrap;
                  overflow: hidden; text-overflow: ellipsis;
                }

                tbody tr { border-bottom: 1px solid rgba(0,0,0,.04); transition: background .1s; cursor: pointer; }
                [data-theme=""dark""] tbody tr { border-bottom-color: rgba(26,74,114,.35); }
                tbody tr:nth-child(even) { background: var(--row-even); }
                tbody tr:hover { background: var(--row-hover); }

                td { padding: 4px 8px; color: var(--text); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }

                .obj { width: auto; }
                .num { color: var(--text-dim); font-size: 10px; }
                .dim { color: var(--text-dim); font-size: 10.5px; }

                .nota-cell { font-weight: 700; font-size: 12px; text-align: center !important; }
                .nh { color: var(--nh); } .nm { color: var(--nm); } .nl { color: var(--nl); }

                .badge {
                  display: inline-block; padding: 1px 6px; border-radius: 20px;
                  font-size: 9.5px; font-weight: 600; letter-spacing: .03em; white-space: nowrap;
                }
                .bpe { background:var(--bpe-bg); color:var(--bpe-c); border:1px solid var(--bpe-b); }
                .bpp { background:var(--bpp-bg); color:var(--bpp-c); border:1px solid var(--bpp-b); }
                .bco { background:var(--bco-bg); color:var(--bco-c); border:1px solid var(--bco-b); }

                .lnk a { color:var(--link); text-decoration:none; font-size:10px; }
                .lnk a:hover { text-decoration:underline; }

                /* STATUS BAR */
                #sb {
                  height: 19px; background: var(--sb);
                  border-top: 1px solid var(--border);
                  display: flex; align-items: center; padding: 0 10px;
                  gap: 10px; font-size: 10px; color: var(--text-dim);
                  flex-shrink: 0; letter-spacing: .04em;
                  transition: background var(--tr), border-color var(--tr);
                }
                .sep { color: var(--border); }
                #cnt { color: var(--text); font-weight: 500; }
                .pgbar { width: 70px; height: 3px; background: var(--border); border-radius: 2px; overflow: hidden; }
                .pgfill { height: 100%; width: 0%; background: var(--prog); border-radius: 2px; transition: width .5s ease; }

                /* ── RESPONSIVO ── */
                @media (max-width: 640px) {
                  html, body { font-size: 11px; }

                  #filters { gap: 4px; }
                  .fi.sm { width: 100px; }
                  .topbar-title span { display: none; }

                  .tab { padding: 0 9px; font-size: 9.5px; }

                  th, td { padding: 4px 5px; font-size: 10px; }
                  .badge { font-size: 9px; padding: 1px 4px; }

                  /* Oculta colunas menos importantes em telas pequenas */
                  .col-local { display: none; }
                }

                @media (max-width: 420px) {
                  .col-dt { display: none; }
                  .fi.sm  { width: 80px; }
                  .tab { padding: 0 7px; font-size: 9px; }
                }
                </style>
                </head>
                <body data-theme=""light"">
                <div id=""app"">

                  <!-- TOPBAR -->
                  <div id=""topbar"">
                    <div id=""logo-wrap"" title=""Visitar site Roque Systems"" onclick=""host('site')"">
                      <img src=""https://roquesystems.com/logo.png"" alt=""Roque Systems""
                           onerror=""this.style.display='none';document.getElementById('logo-fallback').style.display='block'"">
                      <span id=""logo-fallback"">ROQUE SYSTEMS</span>
                    </div>

                    <div class=""topbar-title"">
                      Radar PNCP <span>Monitor de Licitações</span>
                    </div>

                    <div class=""theme-toggle"" onclick=""toggleTheme()"" title=""Alternar tema claro/escuro"">
                      <span>☀️</span>
                      <div class=""toggle-track"">
                        <div class=""toggle-knob"" id=""knob""></div>
                      </div>
                      <span>🌙</span>
                    </div>
                  </div>

                  <div class=""divider""></div>

                  <!-- FILTROS -->
                  <div id=""filters"">
                    <span class=""fl"">Buscar</span>
                    <input class=""fi wide"" id=""f-obj"" placeholder=""Filtrar por objeto ou órgão..."" oninput=""filtrar()"">
                    <span class=""fl"">Modalidade</span>
                    <select class=""fi sm"" id=""f-mod"" onchange=""filtrar()"">
                      <option value="""">Todas</option>
                      <option>Pregão - Eletrônico</option>
                      <option>Pregão - Presencial</option>
                      <option>Concorrência</option>
                    </select>
                    <span class=""fl"">Nota ≥</span>
                    <input class=""fi xs"" id=""f-nota"" type=""number"" min=""0"" max=""10"" placeholder=""0"" oninput=""filtrar()"">
                    <button class=""btn btn-g"" onclick=""limpar()"">Limpar</button>
                    <button class=""btn btn-p"" onclick=""filtrar()"">🔍 Filtrar</button>
                  </div>

                  <!-- ABAS -->
                  <div id=""main"">
                    <div class=""tabs-bar"">
                      <div class=""tab active"" onclick=""switchTab(0,this)""><span class=""tdot""></span>Todas</div>
                      <div class=""tab"" onclick=""switchTab(1,this)""><span class=""tdot""></span>Alta Nota (≥8)</div>
                      <div class=""tab"" onclick=""switchTab(2,this)""><span class=""tdot""></span>Pregão Eletrônico</div>
                    </div>

                    <div class=""tabs-content"">

                      <!-- ABA 0 — Todas -->
                      <div class=""tab-pane active"" id=""p0"">
                        <div class=""tw""><table>
                          <thead><tr>
                            <th style=""width:100px"">Modalidade</th>
                            <th style=""width:140px"">Órgão</th>
                            <th>Objeto</th>
                            <th class=""col-local"" style=""width:90px"">Local</th>
                            <th class=""col-dt"" style=""width:78px"">Atualização</th>
                            <th style=""width:40px;text-align:center"">Nota</th>
                            <th style=""width:46px"">Link</th>
                          </tr></thead>
                          <tbody id=""tb0""></tbody>
                        </table></div>
                      </div>

                      <!-- ABA 1 — Alta Nota -->
                      <div class=""tab-pane"" id=""p1"">
                        <div class=""tw""><table>
                          <thead><tr>
                            <th style=""width:100px"">Modalidade</th>
                            <th style=""width:140px"">Órgão</th>
                            <th>Objeto</th>
                            <th class=""col-local"" style=""width:90px"">Local</th>
                            <th class=""col-dt"" style=""width:78px"">Atualização</th>
                            <th style=""width:40px;text-align:center"">Nota</th>
                            <th style=""width:46px"">Link</th>
                          </tr></thead>
                          <tbody id=""tb1""></tbody>
                        </table></div>
                      </div>

                      <!-- ABA 2 — Pregão Eletrônico -->
                      <div class=""tab-pane"" id=""p2"">
                        <div class=""tw""><table>
                          <thead><tr>
                            <th style=""width:100px"">Modalidade</th>
                            <th style=""width:140px"">Órgão</th>
                            <th>Objeto</th>
                            <th class=""col-local"" style=""width:90px"">Local</th>
                            <th class=""col-dt"" style=""width:78px"">Atualização</th>
                            <th style=""width:40px;text-align:center"">Nota</th>
                            <th style=""width:46px"">Link</th>
                          </tr></thead>
                          <tbody id=""tb2""></tbody>
                        </table></div>
                      </div>

                    </div>
                  </div>

                  <!-- STATUS BAR -->
                  <div id=""sb"">
                    <span>Radar PNCP</span>
                    <span class=""sep"">|</span>
                    <span id=""cnt"">—</span>
                    <span class=""sep"">|</span>
                    <div class=""pgbar""><div class=""pgfill"" id=""pg""></div></div>
                    <span style=""margin-left:auto;font-size:9.5px"">Roque Systems © 2025</span>
                  </div>

                </div>

                <script>


"
            +
            @"
            const DB = " + jsonLicitacoesResumo + @";

            let fil = [...DB];

            function nc(n){ return n>=8?'nh':n>=5?'nm':'nl'; }
            function badge(m){
                if(m.includes('Eletrônico')) return '<span class=""badge bpe"">Pregão-E</span>';
                if(m.includes('Presencial'))  return '<span class=""badge bpp"">Pregão-P</span>';
                return '<span class=""badge bco"">Concorrência</span>';
            }

            function rowAll(r){
                return `<tr onclick=""host('url:${r.Url}')"">
                <td>${badge(r.Modalidade)}</td>
                <td style=""overflow:hidden;text-overflow:ellipsis"">${r.Orgao}</td>
                <td class=""obj"" style=""overflow:hidden;text-overflow:ellipsis"">${r.Objeto}</td>
                <td class=""dim col-local"">${r.Local}</td>
                <td class=""dim col-dt"">${r.Dt}</td>
                <td class=""nota-cell ${nc(r.Nota)}"">${r.Nota}</td>
                <td class=""lnk""><a href=""${r.Url}"" onclick=""event.stopPropagation()"">PNCP ↗</a></td>
                </tr>`;
            }

            function rowSimple(r){
                return `<tr onclick=""host('url:${r.Url}')"">
                <td>${badge(r.Modalidade)}</td>
                <td style=""overflow:hidden;text-overflow:ellipsis"">${r.Orgao}</td>
                <td class=""obj"" style=""overflow:hidden;text-overflow:ellipsis"">${r.Objeto}</td>
                <td class=""dim col-local"">${r.Local}</td>
                <td class=""dim col-dt"">${r.Dt}</td>
                <td class=""nota-cell ${nc(r.Nota)}"">${r.Nota}</td>
                <td class=""lnk""><a href=""${r.Url}"" onclick=""event.stopPropagation()"">PNCP ↗</a></td>
                </tr>`;
            }

            function render(){
                document.getElementById('tb0').innerHTML = fil.map(rowAll).join('');
                document.getElementById('tb1').innerHTML = fil.filter(r=>r.Nota>=8).map(rowSimple).join('');
                document.getElementById('tb2').innerHTML = fil.filter(r=>r.Modalidade.includes('Eletrônico')).map(rowSimple).join('');
                document.getElementById('cnt').textContent = fil.length + ' de ' + DB.length + ' registros';
                const pg = document.getElementById('pg');
                pg.style.width = '0%';
                setTimeout(()=>{ pg.style.width='100%'; }, 30);
            }

            function filtrar(){
                const obj  = document.getElementById('f-obj').value.toLowerCase();
                const mod  = document.getElementById('f-mod').value.toLowerCase();
                const nota = parseInt(document.getElementById('f-nota').value)||0;
                fil = DB.filter(r =>
                (!obj  || r.Objeto.toLowerCase().includes(obj) || r.Orgao.toLowerCase().includes(obj)) &&
                (!mod  || r.Modalidade.toLowerCase().includes(mod)) &&
                r.Nota >= nota
                );
                render();
            }

            function limpar(){
                document.getElementById('f-obj').value = '';
                document.getElementById('f-mod').value = '';
                document.getElementById('f-nota').value = '';
                fil = [...DB]; render();
            }

            // Abas
            const panes = [...document.querySelectorAll('.tab-pane')];
            const tabs  = [...document.querySelectorAll('.tab')];
            function switchTab(i, el){
                panes.forEach(p=>p.classList.remove('active'));
                tabs.forEach(t=>t.classList.remove('active'));
                panes[i].classList.add('active'); el.classList.add('active');
            }

            // Tema
            let dark = false;
            function toggleTheme(){
                dark = !dark;
                document.body.dataset.theme = dark ? 'dark' : 'light';
                document.getElementById('knob').className = 'toggle-knob' + (dark?' r':'');
            }

            // Comunicação com WebView2 host
            function host(msg){
                try { window.chrome.webview.postMessage(msg); } catch(e){}
            }

            // ── LOCKDOWN COMPLETO WEBVIEW2 ──
            // 1. Botão direito
            document.addEventListener('contextmenu', e => e.preventDefault());

            // 2. Seleção de texto via teclado (Shift+setas, Ctrl+A etc.) — exceto em inputs
            document.addEventListener('selectstart', e => {
                if (!['INPUT','TEXTAREA','SELECT'].includes(e.target.tagName)) e.preventDefault();
            });

            // 3. Arrastar elementos
            document.addEventListener('dragstart', e => e.preventDefault());

            // 4. Atalhos de navegador
            document.addEventListener('keydown', e => {
                const k = e.key.toUpperCase();
                const c = e.ctrlKey || e.metaKey;
                const bloqueados =
                e.key === 'F5'  ||
                e.key === 'F12' ||
                e.key === 'F3'  ||
                (c && k === 'R') ||
                (c && k === 'F') ||
                (c && k === 'U') ||
                (c && k === 'S') ||
                (c && k === 'P') ||
                (c && k === 'D') ||
                (c && k === 'N') ||
                (c && k === 'T') ||
                (c && k === 'W') ||
                (c && k === 'H') ||
                (c && k === 'J') ||
                (c && k === 'L') ||
                (c && k === 'E') ||
                (c && k === 'G') ||
                (c && e.shiftKey && k === 'I') ||
                (c && e.shiftKey && k === 'J') ||
                (c && e.shiftKey && k === 'C') ||
                (c && e.shiftKey && k === 'K') ||
                (c && e.shiftKey && k === 'U') ||
                (e.altKey && e.key === 'ArrowLeft')  ||
                (e.altKey && e.key === 'ArrowRight') ||
                (e.altKey && e.key === 'F4');
                if (bloqueados) e.preventDefault();
            });

            // 5. Zoom via scroll (Ctrl+Scroll)
            document.addEventListener('wheel', e => {
                if (e.ctrlKey) e.preventDefault();
            }, { passive: false });

            // 6. Zoom via pinch (touch)
            document.addEventListener('touchstart', e => {
                if (e.touches.length > 1) e.preventDefault();
            }, { passive: false });
            document.addEventListener('touchmove', e => {
                if (e.touches.length > 1) e.preventDefault();
            }, { passive: false });

            render();
            </script>
            </body>
            </html>            
            ";

            return resposta;
        }
    }
}