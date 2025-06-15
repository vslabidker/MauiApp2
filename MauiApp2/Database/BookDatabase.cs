using SQLite;
using MauiApp2.Models;

namespace MauiApp2.Database;

public class BookDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public BookDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<SavedBook>().Wait();
    }

    public Task<int> SaveBookAsync(SavedBook book) => _database.InsertAsync(book);

    public Task<List<SavedBook>> GetBooksAsync() => _database.Table<SavedBook>().ToListAsync();

    public Task<int> DeleteBookAsync(SavedBook book) => _database.DeleteAsync(book);
}
