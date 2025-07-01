using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NFCEApp.Models;

namespace NFCEApp.Services
{
    public class NotaFiscalApiService
    {
        private readonly HttpClient _http;

        public NotaFiscalApiService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.104:5175/") // Substitua pela sua API real
            };
        }

        public async Task<List<NotaFiscal>> GetNotasAsync()
        {
            List <NotaFiscal> notas = new List<NotaFiscal>();
            try
            {
                var response = await _http.GetAsync("api/NotaFiscal");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NotaFiscal>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // Exiba o erro no Debug ou na interface
                Debug.WriteLine($"ERRO: {ex}");
                return new List<NotaFiscal>();
            }
        }
        // Find notas
        public async Task<NotaFiscal> FindNotasAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"api/NotaFiscal/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NotaFiscal>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao buscar notas: {ex}");
                return new NotaFiscal();
            }
        }
        public async Task<NotaFiscal?> PostNotaAsync(NotaFiscal nota)
        {
            try
            {
                var json = JsonSerializer.Serialize(nota);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync("api/NotaFiscal", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // Desserializa a nota retornada (com o ID gerado na API)
                var notaCriada = JsonSerializer.Deserialize<NotaFiscal>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return notaCriada;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao enviar nota: {ex}");
                return null;
            }
        }
        public async Task<bool> DeleteNotaAsync(NotaFiscal nota)
        {
            if (nota.idApi != 0)
            {
                try
                {
                    var json = JsonSerializer.Serialize(nota);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    int id = nota.idApi??0;
                    var response = await _http.DeleteAsync($"api/NotaFiscal/{id}");
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao enviar nota: {ex}");
                    return false;
                }
            }
            return false;
        }
    }
}
