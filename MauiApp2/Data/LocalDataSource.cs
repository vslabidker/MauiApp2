using MauiApp2.Models;
using MauiApp2.Database;

namespace MauiApp2.Data;

public class LocalDataSource : ILocalDataSource
{
    private readonly BookDatabase _db;

    public LocalDataSource(string dbPath)
    {
        _db = new BookDatabase(dbPath);
    }

    public Task<List<SavedBook>> GetSavedBooksAsync() => _db.GetBooksAsync();

    public Task SaveBookAsync(SavedBook book) => _db.SaveBookAsync(book);

    public Task DeleteBookAsync(SavedBook book) => _db.DeleteBookAsync(book);
}
