using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using StockPortfolio.Models;
namespace StockPortfolio
{
    public partial class news : ContentPage
    {
        private string NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        public HttpClient BingNewsSearchClient
        {
            get;
            set;
        }
        public news()
        {
            InitializeComponent();
            BingNewsSearchClient = new HttpClient();
            BingNewsSearchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7d3f09aae7154bea8a4b17de9778bb04");
        }
        private async void OnTextChangesEvent(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (entrysearch != null)
                    lstnews.ItemsSource = await GetNewsSearchResults(this.entrysearch.Text);
            }
            catch (HttpRequestException)
            {

            }

        }
        public async Task<IEnumerable<NewsArticle>> GetNewsSearchResults(string query)
        {
            int count = 20;
            int offset = 0;
            string market = "en-US";

            List<NewsArticle> articles = new List<NewsArticle>();
            var result = await RequestAndAutoRetryWhenThrottled(() => BingNewsSearchClient.GetAsync(string.Format("{0}/?q={1}&count={2}&offset={3}&mkt={4}", NewsSearchEndPoint, WebUtility.UrlEncode(query), count, offset, market)));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);

            if (data.value != null && data.value.Count > 0)
            {
                for (int i = 0; i < data.value.Count; i++)
                {
                    articles.Add(new NewsArticle
                    {
                        Title = data.value[i].name,
                        Url = data.value[i].url,
                        Description = data.value[i].description,
                        ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
                        Provider = data.value[i].provider?[0].name
                    });
                }
            }
            return articles;
        }
        private async Task<HttpResponseMessage> RequestAndAutoRetryWhenThrottled(Func<Task<HttpResponseMessage>> action)
        {
            int retriesLeft = 6;
            int delay = 500;

            HttpResponseMessage response = null;

            while (true)
            {
                response = await action();

                if ((int)response.StatusCode == 429 && retriesLeft > 0)
                {
                    await Task.Delay(delay);
                    retriesLeft--;
                    delay *= 2;
                    continue;
                }
                else
                {
                    break;
                }
            }

            return response;
        }
    }
}

