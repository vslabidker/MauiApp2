using SQLite;

namespace MauiApp2.Models;

public class SavedBook
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Thumbnail { get; set; }
}
