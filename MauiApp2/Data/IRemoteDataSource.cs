using MauiApp2.Models;

namespace MauiApp2.Data;

public interface IRemoteDataSource
{
    Task<List<BookItem>> SearchBooksAsync(string query);
}
