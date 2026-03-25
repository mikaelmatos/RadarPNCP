# Radar PNCP

Sistema automatizado para monitoramento e análise de oportunidades no PNCP (Portal Nacional de Contratações Públicas), utilizando web scraping e inteligência artificial para identificar licitações compatíveis com empresas cadastradas.

---

# Não sabe programar?

Apenas execute o .exe e cadastre a empresa, os termos de busca e siga o manunal abaixo para que rode automaticamente.
https://lembrar_de_botar_o_link.com


## Visão Geral

O Radar PNCP foi desenvolvido com o objetivo de:

- Coletar dados de licitações diretamente do PNCP
- Filtrar resultados com base em termos de interesse previamente cadastrados
- Analisar automaticamente a compatibilidade entre a licitação e o perfil da empresa utilizando IA
- Gerar dados estruturados para integração, relatórios ou automações

---

## Funcionalidades

### Web Scraping Automatizado
- Extração de licitações diretamente do PNCP
- Suporte a paginação e grandes volumes de dados

### Filtro por Palavras-chave
- Termos personalizados por empresa
- Redução de ruído nos dados coletados

### Análise com Inteligência Artificial
- Avaliação da aderência da licitação ao perfil da empresa
- Score de compatibilidade (ex: 0 a 10)

### Persistência de Dados
- Armazenamento em banco SQL
- Controle de duplicidade (ex: por Id PNCP)

### Exportação
- Geração de relatórios em CSV
