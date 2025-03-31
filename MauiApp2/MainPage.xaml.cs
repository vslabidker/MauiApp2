using System;
using RestSharp;
using System.Threading.Tasks;

namespace MauiApp2;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnFetchDataClicked(object sender, EventArgs e)
    {

        string query = SearchEntry.Text;

        if (string.IsNullOrEmpty(query))
        {
            await DisplayAlert("Ошибка", "Введите запрос", "OK");
            return;
        }

        var client = new RestClient("https://api.apilayer.com/spoonacular/recipes/complexSearch");
        var request = new RestRequest();
        request.AddParameter("query", query); 
        request.AddHeader("apikey", "lBk76uWoDkHbjMQ9jSUuuxQZOGTglsGj");

        try
        {
            
            var response = await client.ExecuteAsync(request); 

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            if (response.IsSuccessful)
            {
                await DisplayAlert("Ответ API", response.Content, "OK");
            }
            else
            {
                await DisplayAlert("Ошибка", "Не удалось получить данные с API", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            await DisplayAlert("Ошибка", "Произошла ошибка при запросе", "OK");
        }
    }
}
