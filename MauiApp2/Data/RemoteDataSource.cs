using Newtonsoft.Json;
using MauiApp2.Models;

namespace MauiApp2.Data;

public class RemoteDataSource : IRemoteDataSource
{
    public async Task<List<BookItem>> SearchBooksAsync(string query)
    {
        var httpClient = new HttpClient();
        string url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(query)}";

        var response = await httpClient.GetStringAsync(url);
        var booksResponse = JsonConvert.DeserializeObject<GoogleBooksResponse>(response);

        var books = new List<BookItem>();

        if (booksResponse?.Items != null)
        {
            foreach (var item in booksResponse.Items)
            {
                var title = item.VolumeInfo?.Title;
                var thumb = item.VolumeInfo?.ImageLinks?.Thumbnail;

                if (!string.IsNullOrEmpty(title))
                {
                    books.Add(new BookItem { Title = title, Thumbnail = thumb });
                }
            }
        }

        return books;
    }

    class GoogleBooksResponse
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    class Item
    {
        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }

    class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }
    }

    class ImageLinks
    {
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
