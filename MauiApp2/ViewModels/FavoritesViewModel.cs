using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp2.Models;
using MauiApp2.Database;

namespace MauiApp2.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly BookDatabase _database;
    public ObservableCollection<SavedBook> Favorites { get; set; } = new();

    public ICommand LoadCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SelectionChangedCommand { get; }

    private IList<object> _selectedItems = new List<object>();
    public IList<object> SelectedItems
    {
        get => _selectedItems;
        set
        {
            SetProperty(ref _selectedItems, value);
            IsDeleteVisible = _selectedItems.Any();
            SelectedBook = _selectedItems.FirstOrDefault() as SavedBook;
        }
    }

    private bool _isDeleteVisible;
    public bool IsDeleteVisible
    {
        get => _isDeleteVisible;
        set => SetProperty(ref _isDeleteVisible, value);
    }

    public FavoritesViewModel(BookDatabase database)
    {
        _database = database;
        LoadCommand = new Command(async () => await LoadFavoritesAsync());
        DeleteCommand = new Command(async () => await DeleteSelectedAsync());
        SelectionChangedCommand = new Command<IList<object>>(OnSelectionChanged);
    }

    private void OnSelectionChanged(IList<object> items)
    {
        SelectedItems = items ?? new List<object>();
    }

    public async Task LoadFavoritesAsync()
    {
        var books = await _database.GetBooksAsync();
        Favorites.Clear();
        foreach (var book in books)
            Favorites.Add(book);
    }

    private async Task DeleteSelectedAsync()
    {
        if (!SelectedItems.Any()) return;

        var toDelete = SelectedItems.Cast<SavedBook>().ToList();
        bool confirm = await Shell.Current.DisplayAlert("Удалить", $"Удалить {toDelete.Count} книг(и)?", "Да", "Нет");

        if (!confirm) return;

        foreach (var book in toDelete)
        {
            await _database.DeleteBookAsync(book);
            Favorites.Remove(book);
        }

        SelectedItems = new List<object>();
        IsDeleteVisible = false;
    }

    private SavedBook _selectedBook;
    public SavedBook SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }

    private bool _isLandscape;
    public bool IsLandscape
    {
        get => _isLandscape;
        set => SetProperty(ref _isLandscape, value);
    }
}
