using MauiApp2.Models;

namespace MauiApp2.Data;

public class BookRepository : IBookRepository
{
    private readonly IRemoteDataSource _remote;
    private readonly ILocalDataSource _local;

    public BookRepository(IRemoteDataSource remote, ILocalDataSource local)
    {
        _remote = remote;
        _local = local;
    }

    public Task<List<BookItem>> SearchBooksAsync(string query) => _remote.SearchBooksAsync(query);

    public Task<List<SavedBook>> GetFavoritesAsync() => _local.GetSavedBooksAsync();

    public Task SaveToFavoritesAsync(BookItem book)
    {
        var saved = new SavedBook { Title = book.Title, Thumbnail = book.Thumbnail };
        return _local.SaveBookAsync(saved);
    }

    public Task DeleteFromFavoritesAsync(SavedBook book) => _local.DeleteBookAsync(book);
}
