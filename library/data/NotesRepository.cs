using library.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace library.data;

// Every SQL statement about notes lives in this one class.
public class NotesRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public NotesRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    // Copies the current row into a Notes object.
    private static Note ReadNote(NpgsqlDataReader reader)
    {
        return new Note
        {
            note_id = reader.GetInt64(0),
            title = reader.IsDBNull(1) ? "" : reader.GetString(1),
            body = reader.IsDBNull(2) ? "" : reader.GetString(2),
            created_on = reader.IsDBNull(3)
                ? null
                : reader.GetDateTime(3).ToString("yyyy-MM-dd")
        };
    }

    // READ: every note, sorted by title.
    public async Task<List<Note>> GetAllAsync()
    {
        const string sql =
            "SELECT note_id, title, body, created_on " +
            "FROM lending.note ORDER BY title;";

        var notes = new List<Note>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            notes.Add(ReadNote(reader));
        }

        return notes;
    }

    private readonly NotesRepository _notes;

}