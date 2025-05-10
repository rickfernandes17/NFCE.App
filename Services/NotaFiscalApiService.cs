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
                BaseAddress = new Uri("http://192.168.0.100:5175/") // Substitua pela sua API real
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
    }
}
