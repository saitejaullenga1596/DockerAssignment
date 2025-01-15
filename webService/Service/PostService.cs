using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using webservice.Interface;
using webservice.Model;

namespace webservice.Service
{
    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private readonly string _cacheBaseUri;
        private readonly string _dataBaseUri;
        private readonly ILogger<PostService> _logger;

        public PostService(HttpClient httpClient, IOptions<CacheServiceConfiguration> cacheServiceConfiguration, IOptions<DataServiceConfiguration> dataServiceConfiguration,ILogger<PostService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cacheBaseUri = cacheServiceConfiguration.Value.BaseUri ?? throw new ArgumentNullException(nameof(_cacheBaseUri));
            _dataBaseUri = dataServiceConfiguration.Value.BaseUri ?? throw new ArgumentNullException(nameof(_dataBaseUri));
            _logger = logger;
        }

        public async Task<Post?> GetPost(int id)
        {
            try
            {
                // get response from cache service instead of data service
                var cacheResponse = await _httpClient.GetAsync($"{_cacheBaseUri}api/cache/get/{id}");
                if (cacheResponse.IsSuccessStatusCode)
                {
                    var cachedData = await cacheResponse.Content.ReadAsStringAsync();
                    var post = JsonConvert.DeserializeObject<Post>(cachedData);
                    if (post != null)
                    {
                        return post;
                    }
                }
                // get data from dataservice if cache service not have its value
                var dataResponse = await _httpClient.GetAsync($"{_dataBaseUri}api/PostData/{id}");
                if (dataResponse.IsSuccessStatusCode)
                {
                    var dbData = await dataResponse.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Post>(dbData);
                    return book;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreatePost(Post model)
        {
            try
            {
                if (_dataBaseUri != null)
                {
                    // create post in data service
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var success = await _httpClient.PostAsync($"{_dataBaseUri}api/PostData/{model.Id}", content);
                    if (success.IsSuccessStatusCode)
                    {
                        // save post value in cache service
                        var cacheClient = await _httpClient.PostAsync($"{_cacheBaseUri}api/cache/save/{model.Id}", content);
                        if (cacheClient.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogInformation("Something went wrong in cache service unable to save post.");
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
