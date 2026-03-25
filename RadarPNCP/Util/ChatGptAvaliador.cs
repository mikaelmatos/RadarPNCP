using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RadarPNCP.Util
{
    public class ChatGptAvaliador
    {
        public static async Task<double> AvaliarTextoAsync(string promptBase, string texto)
        {

            StreamReader reader = new StreamReader("chaves-gpt.mik");
            string chaves = await reader.ReadToEndAsync();
            reader.Close();

            List<string> listaKeys = chaves.Replace("\r", "").Split("\n").ToList();

            string chaveAPI = listaKeys[(int)(new Random().Next(0, listaKeys.Count - 1))];

            try
            {
                using var http = new HttpClient();

                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", chaveAPI);

                var promptFinal = $"{promptBase}\n\n{texto}";

                var body = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                new { role = "system", content = "Você é um avaliador que responde apenas números de 0 a 10." },
                new { role = "user", content = promptFinal }
            },
                    temperature = 0.2
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseString);
                var respostaTexto = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                // Tenta converter pra número
                if (double.TryParse(respostaTexto.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double nota))
                {
                    return nota;
                }

                return -1; // erro de parsing
            }
            catch
            {
                return -2;
            }
        }
    }
}
