using RadarPNCP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RadarPNCP.Repository
{
    public class LicitacaoResumoRepository
    {
        public static async Task<bool> InserirSeNaoExistirAsync(LicitacaoResumo licitacao)
        {
            string connectionString = "Data Source=DESKTOP-P93N2NG\\SQLEXPRESS;Initial Catalog=RadarPNCP;Integrated Security=True;TrustServerCertificate=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var query = @"
IF NOT EXISTS (SELECT 1 FROM LicitacaoResumo WHERE IdPNCP = @IdPNCP)
BEGIN
    INSERT INTO LicitacaoResumo
    (
        UrlDetalhes,
        IdPNCP,
        Modalidade,
        Orgao,
        Objeto,
        Local,
        UltimaAtualizacao,
        Nota
    )
    VALUES
    (
        @UrlDetalhes,
        @IdPNCP,
        @Modalidade,
        @Orgao,
        @Objeto,
        @Local,
        @UltimaAtualizacao,
        @Nota
    )

    SELECT 1
END
ELSE
BEGIN
    SELECT 0
END
";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UrlDetalhes", (object?)licitacao.UrlDetalhes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdPNCP", (object?)licitacao.IdPNCP ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Modalidade", (object?)licitacao.Modalidade ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Orgao", (object?)licitacao.Orgao ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Objeto", (object?)licitacao.Objeto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Local", (object?)licitacao.Local ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UltimaAtualizacao", licitacao.UltimaAtualizacao);
                    cmd.Parameters.AddWithValue("@Nota", licitacao.Nota);

                    var result = (int)await cmd.ExecuteScalarAsync();

                    return result == 1; // true = inseriu, false = já existia
                }
            }
        }

        public static async Task<bool> ExistePorIdPncpAsync(string idPncp)
        {
            string connectionString = "Data Source=DESKTOP-P93N2NG\\SQLEXPRESS;Initial Catalog=RadarPNCP;Integrated Security=True;TrustServerCertificate=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var query = @"
IF EXISTS (SELECT 1 FROM LicitacaoResumo WHERE IdPNCP = @IdPNCP)
    SELECT 1
ELSE
    SELECT 0
";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPNCP", (object?)idPncp ?? DBNull.Value);

                    var result = (int)await cmd.ExecuteScalarAsync();

                    return result == 1; // true = existe, false = não existe
                }
            }
        }

        public static async Task<List<LicitacaoResumo>> ListarAsync(
    string? modalidade = null,
    string? orgao = null,
    DateTime? dataInicio = null,
    DateTime? dataFim = null,
    int pagina = 1,
    int tamanhoPagina = 5000)
        {
            string connectionString = "Data Source=DESKTOP-P93N2NG\\SQLEXPRESS;Initial Catalog=RadarPNCP;Integrated Security=True;TrustServerCertificate=True;";

            var resultado = new List<LicitacaoResumo>();

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var query = new StringBuilder(@"
            SELECT
                UrlDetalhes,
                IdPNCP,
                Modalidade,
                Orgao,
                Objeto,
                Local,
                UltimaAtualizacao,
                Nota
            FROM LicitacaoResumo
            WHERE 1=1
        ");

                if (!string.IsNullOrWhiteSpace(modalidade))
                    query.Append(" AND Modalidade = @Modalidade");

                if (!string.IsNullOrWhiteSpace(orgao))
                    query.Append(" AND Orgao = @Orgao");

                if (dataInicio.HasValue)
                    query.Append(" AND UltimaAtualizacao >= @DataInicio");

                if (dataFim.HasValue)
                    query.Append(" AND UltimaAtualizacao <= @DataFim");

                query.Append(@"
            ORDER BY UltimaAtualizacao DESC
            OFFSET @Offset ROWS
            FETCH NEXT @TamanhoPagina ROWS ONLY
        ");

                using (var cmd = new SqlCommand(query.ToString(), conn))
                {
                    if (!string.IsNullOrWhiteSpace(modalidade))
                        cmd.Parameters.AddWithValue("@Modalidade", modalidade);

                    if (!string.IsNullOrWhiteSpace(orgao))
                        cmd.Parameters.AddWithValue("@Orgao", orgao);

                    if (dataInicio.HasValue)
                        cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value);

                    if (dataFim.HasValue)
                        cmd.Parameters.AddWithValue("@DataFim", dataFim.Value);

                    cmd.Parameters.AddWithValue("@Offset", (pagina - 1) * tamanhoPagina);
                    cmd.Parameters.AddWithValue("@TamanhoPagina", tamanhoPagina);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new LicitacaoResumo
                            {
                                UrlDetalhes = reader["UrlDetalhes"] as string,
                                IdPNCP = reader["IdPNCP"] as string,
                                Modalidade = reader["Modalidade"] as string,
                                Orgao = reader["Orgao"] as string,
                                Objeto = reader["Objeto"] as string,
                                Local = reader["Local"] as string,
                                UltimaAtualizacao = (DateTime)reader["UltimaAtualizacao"],
                                Nota = (int)reader["Nota"]
                            });
                        }
                    }
                }
            }

            return resultado;
        }
    }
}
