using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GroceryList.Model;
using Newtonsoft.Json;

namespace GroceryList.Service
{
    public class GroceryService
    {
        HttpClient client;
        const string url = "http://expensesmonitoringapi2.azurewebsites.net/api/GroceryItems";
        public GroceryService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<GroceryItems>> RefreshDataAsync()
        {
            List<GroceryItems> Items = new List<GroceryItems>();
            var uri = new Uri(url);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Items = JsonConvert.DeserializeObject<List<GroceryItems>>(content);
            }
            return Items;
        }

        public async Task SaveGroceryItemAsync(GroceryItems item)
        {

            var uri = new Uri(url);
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"TodoItem successfully saved.");

            }
        }

        public async Task DeleteGroceryItemAsync(int id)
        {
            var uri = new Uri(string.Format(url + "/{0}", id));
            var response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"TodoItem successfully deleted.");
            }
        }
    }
}
