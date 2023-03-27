using Newtonsoft.Json;
using System.Text;
using Watch.NovaPoshta.Models;

namespace Watch.NovaPoshta
{
    public class NovaPoshtaAddress
    {
        private readonly string _apiKey;
        private readonly string _url;
        public NovaPoshtaAddress(string apiKey, string url)
        {
            _apiKey = apiKey;
            _url = url;
        }


        public async Task<List<CityModel>> GetCitiesAsync()
        {
            List<CityModel> cities = new List<CityModel>();
            HttpClient client = new HttpClient();

            RequestBodyContent content = new RequestBodyContent
            {
                apiKey = _apiKey,
                calledMethod = "getCities",
                modelName = "Address"
            };

            var contentJson = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_url, httpContent);

            if (response.Content != null)
            {
                var responseContentString = await response.Content.ReadAsStringAsync();
                var responseContent = JsonConvert.DeserializeObject<ResponseBodyContent<CityModel>>(responseContentString);

                if (responseContent != null && responseContent.success == true && responseContent.data != null)
                {
                    cities.AddRange(responseContent.data);
                }
            }

            return cities;
        }


        public async Task<List<WarehouseModel>> GetWarehousesAsync(string? type = null)
        {
            List<WarehouseModel> warehouses = new List<WarehouseModel>();
            HttpClient client = new HttpClient();

            RequestBodyContent content = new RequestBodyContent
            {
                apiKey = _apiKey,
                calledMethod = "getWarehouses",
                modelName = "Address"
            };

            if(type != null)
            {
                content.methodProperties = new
                {
                    TypeOfWarehouseRef = type
                };
            }

            var contentJson = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(_url, httpContent);

                if (response.Content != null)
                {
                    var responseContentString = await response.Content.ReadAsStringAsync();
                    var responseContent = JsonConvert.DeserializeObject<ResponseBodyContent<WarehouseModel>>(responseContentString);

                    if (responseContent != null && responseContent.success == true && responseContent.data != null)
                    {
                        warehouses.AddRange(responseContent.data);
                    }
                }
            }
            catch
            {

            }

            return warehouses;
        }


    }
}
