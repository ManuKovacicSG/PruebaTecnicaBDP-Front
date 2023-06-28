using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorCRUD.model
{
    public class FilmsService : FilmsInterface
    {
        private readonly HttpClient _httpClient;
        const string _baseUrl = "http://albertbdp.somee.com/api/peliculas";
        const string _filmsEndpoint = "http://albertbdp.somee.com/api/peliculas";
        const string _host = "albertbdp.somee.com";


        public FilmsService(HttpClient httpClient) => _httpClient = httpClient;


        public async Task<List<FilmItem>> GetFilms(List<FilmItem> filmItem)
        {
            ConfigureHttpClient();

            var response = await _httpClient.GetAsync(_filmsEndpoint);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();

            var dto = await JsonSerializer.DeserializeAsync<FilmsDto>(stream);
            return dto.data.FirstOrDefault().name.Select(n => new FilmItem 
            { 
                Name = n.name,
                Gendre = n.Gendre,
            }).ToList();

        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Films", _host);
        }
    }
}
