using MauiApp2.Models;

namespace MauiApp2.Data;

public interface ILocalDataSource
{
    Task<List<SavedBook>> GetSavedBooksAsync();
    Task SaveBookAsync(SavedBook book);
    Task DeleteBookAsync(SavedBook book);
}
