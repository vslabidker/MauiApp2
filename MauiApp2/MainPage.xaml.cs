using System;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            var recipeResponse = JsonConvert.DeserializeObject<MenuItems>(response.Content);

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");



            if (response.IsSuccessful)
            {   await DisplayAlert("Відповідь",$"{response.Content}", "OK");
                Console.WriteLine($"First Recipe Title: {recipeResponse.Results[0].Title}");
                Console.WriteLine($"First Recipe Image URL: {recipeResponse.Results[0].Image}");
                var image = new Image() { Source = recipeResponse.Results[0].Image };

                recipeTitleLabel.Text = recipeResponse.Results[0].Title;
                recipeTitleLabel.IsVisible = true;

                recipeImage.Source = recipeResponse.Results[0].Image;
                recipeImage.IsVisible = true;
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
