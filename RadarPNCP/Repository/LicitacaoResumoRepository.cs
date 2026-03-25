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
    }
}
