using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using AltNetBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AltNetBot.Services
{
    public class MeetupService
    {
        private const string BaseUrl = "https://api.meetup.com";
        private readonly string _apiKey = $"?key={ConfigurationManager.AppSettings["MeetupApiKey"]}&sign=true";

        public async Task<IList<Group>> FindGroups(string location, int? categoryId)
        {
            var query = $"/find/groups{_apiKey}&location={location}&page=5";
            if (categoryId.HasValue)
                query += $"&category={categoryId}";

            return await ExecuteQueryAsync<IList<Group>>(query);
        }

        public async Task<IList<Category>> ListCategories()
        {
            var query = $"/2/categories{_apiKey}&format=json&photo-host=public&order=shortname&desc=false";

            return (await ExecuteQueryAsync<CategoryResult>(query))?.Results;
        }

        internal static async Task<T> ExecuteQueryAsync<T>(string queryUrl, HttpMethod method = null, HttpContent content = null)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromMilliseconds(30000) };
            HttpResponseMessage result;

            if (method == HttpMethod.Post)
                result = await httpClient.PostAsync(queryUrl, content);
            else if (method == HttpMethod.Put)
                result = await httpClient.PutAsync(queryUrl, content);
            else if (method == HttpMethod.Delete)
                result = await httpClient.DeleteAsync(queryUrl);
            else
                result = await httpClient.GetAsync(queryUrl);

            result.EnsureSuccessStatusCode();

            return await ProcessJson<T>(result.Content);
        }

        private static T ProcessJson<T>(string content)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Argument is null or empty", nameof(content));

            var deserializedData = JsonConvert.DeserializeObject<T>(content, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            return deserializedData;
        }

        private static async Task<T> ProcessJson<T>(HttpContent content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var json = await content.ReadAsStringAsync();
            if (json.StartsWith("<!DOCTYPE html>"))
            {
                return default(T);
            }

            var result = ProcessJson<T>(json);
            return result;
        }
    }
}