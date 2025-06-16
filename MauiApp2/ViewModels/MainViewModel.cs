using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp2.Data;
using MauiApp2.Models;

namespace MauiApp2.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IBookRepository _repository;

    public ObservableCollection<BookItem> Books { get; set; } = new();
    public ICommand SearchCommand { get; }
    public ICommand SaveCommand { get; }

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    private bool _isSaveVisible;
    public bool IsSaveVisible
    {
        get => _isSaveVisible;
        set => SetProperty(ref _isSaveVisible, value);
    }

    public MainViewModel(IBookRepository repository)
    {
        _repository = repository;
        SearchCommand = new Command(async () => await SearchBooksAsync());
        SaveCommand = new Command(async () => await SaveBooksAsync());
    }

    private async Task SearchBooksAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

        var result = await _repository.SearchBooksAsync(SearchQuery);

        Books.Clear();
        foreach (var book in result)
            Books.Add(book);

        IsSaveVisible = Books.Any();
    }

    private async Task SaveBooksAsync()
    {
        var selected = Books.Where(b => b.IsSelected).ToList();
        if (!selected.Any())
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите книги", "OK");
            return;
        }

        foreach (var book in selected)
            await _repository.SaveToFavoritesAsync(book);

        await Shell.Current.DisplayAlert("Готово", "Книги сохранены", "OK");
    }
}
