using MauiApp2.Models;

namespace MauiApp2.Data;

public interface IBookRepository
{
    Task<List<BookItem>> SearchBooksAsync(string query);
    Task<List<SavedBook>> GetFavoritesAsync();
    Task SaveToFavoritesAsync(BookItem book);
    Task DeleteFromFavoritesAsync(SavedBook book);
}
