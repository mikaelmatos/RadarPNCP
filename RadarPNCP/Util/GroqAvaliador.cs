using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RadarPNCP.Util
{
    public class GroqAvaliador
    {
        public static List<string> listaKeys = new List<string>();

        public static async Task<double> AvaliarTextoAsync(string promptBase, string texto)
        {
            StreamReader reader = new StreamReader("chaves-grok.mik");
            string chaves = await reader.ReadToEndAsync();
            reader.Close();

            listaKeys = chaves.Replace("\r", "").Split("\n").ToList();

            try
            {
                using var http = new HttpClient();

                int id = (int)(new Random().Next(0, listaKeys.Count - 1));
                string chave = listaKeys[id];

                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", chave);

                var promptFinal = $"{promptBase}\n\n{texto}";

                var body = new
                {
                    model = "llama-3.1-8b-instant", // rápido e gratuito
                    messages = new[]
                    {
                new { role = "system", content = "Você é um avaliador que responde apenas números sobre a compatibilidade do resumo com os sistemas listados, nota de 0 a 10." },
                new { role = "user", content = promptFinal }
            },
                    temperature = 0.2
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseString);
                var respostaTexto = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (double.TryParse(respostaTexto?.Trim().Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double nota))
                {
                    return nota;
                }

                return -1;
            }
            catch (Exception ex)
            {
                await Task.Delay(10000);
                Console.WriteLine($"Erro ao avaliar texto: {ex.Message}");
                return -1;
            }
        }
    }
}
