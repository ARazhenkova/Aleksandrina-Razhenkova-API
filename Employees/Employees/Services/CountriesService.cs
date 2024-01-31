using EmployeesApi.Data;
using EmployeesApi.Models.ExternalSystems.Countries;
using EmployeesApi.ModelToDataMappers;
using EmployeesApi.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace EmployeesApi.Services
{
    public interface ICountriesService
    {
        Task<Country?> FindByAlpha2CodeAsync(string alpha2Code);
    }

    public class CountriesService : ICountriesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public CountriesService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<Country?> FindByAlpha2CodeAsync(string alpha2Code)
        {
            Country? country = null;

            if (!String.IsNullOrWhiteSpace(alpha2Code))
            {
                List<Country> countries = new List<Country>();

                if (_memoryCache.TryGetValue(MemoryCacheKeys.COUNTRIES, out List<Country>? value) || value != null)
                {
                    countries = value;
                }
                else
                {
                    countries = await LoadCountriesAsync();
                }

                country = countries.Find(c => c.AlphaCode2.ToUpper() == alpha2Code.ToUpper());
            }

            return country;
        }

        private async Task<List<Country>> LoadCountriesAsync()
        {
            List<Country> countries = await SendRequestAsync();

            if (!_memoryCache.TryGetValue(MemoryCacheKeys.COUNTRIES, out Dictionary<string, Country>? value) || value == null)
                _memoryCache.Set(MemoryCacheKeys.COUNTRIES, countries);

            return countries;
        }

        private async Task<List<Country>> SendRequestAsync()
        {
            string? url = _configuration.GetValue<string>(AppConfigurations.LOADING_COUNTRIES_URL);
            if (url == null || String.IsNullOrWhiteSpace(url))
                throw new ArgumentException($@"The URL for loading countries is not configured in the ""{AppConfigurations.GetFileName()}"" at ""{AppConfigurations.LOADING_COUNTRIES_URL}"".");

            HttpClient httpClient = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($@"""{url}"" is not responding.");

            string? responseBody = await response.Content.ReadAsStringAsync();

            LoadCountriesOutputModel? outputModel = JsonSerializer.Deserialize<LoadCountriesOutputModel>(responseBody);
            if (outputModel == null || outputModel.data.Count == 0)
                throw new ArgumentException($@"""{url}"" is responding with no data.");

            List<Country> countries = new List<Country>();
            outputModel.data.ForEach(countryModel =>
            {
                Country country = CountryMapper.Map(countryModel);
                countries.Add(country);
            });

            return countries;
        }
    }
}
