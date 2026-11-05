
using library.Models;
using Npgsql;

namespace library.data
{
    public class NotesRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public NotesRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        // Convert a database row into a Note object.
        private static Note ReadNote(NpgsqlDataReader reader)
        {
            return new Note
            {
                note_id = reader.GetInt64(0),
                title = reader.IsDBNull(1) ? "" : reader.GetString(1),
                body = reader.IsDBNull(2) ? null : reader.GetString(2),
                created_on = reader.IsDBNull(3)
                    ? null
                    : reader.GetDateTime(3).ToString("yyyy-MM-dd")
            };
        }

        // GET ALL NOTES
        public async Task<List<Note>> GetAllAsync()
        {
            const string sql = @"
                SELECT note_id, title, body, created_on
                FROM lending.note
                ORDER BY title;";

            var notes = new List<Note>();

            await using var command = _dataSource.CreateCommand(sql);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                notes.Add(ReadNote(reader));
            }

            return notes;
        }

        // GET NOTE BY ID
        public async Task<Note?> GetByIdAsync(long id)
        {
            const string sql = @"
                SELECT note_id, title, body, created_on
                FROM lending.note
                WHERE note_id = @id;";

            await using var command = _dataSource.CreateCommand(sql);
            command.Parameters.AddWithValue("id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return ReadNote(reader);
            }

            return null;
        }

        // CREATE NOTE
        public async Task AddAsync(Note note)
        {
            const string sql = @"
                INSERT INTO lending.note (title, body)
                VALUES (@title, @body);";

            await using var command = _dataSource.CreateCommand(sql);

            command.Parameters.AddWithValue("title", note.title);
            command.Parameters.AddWithValue(
                "body", (object?)note.body ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        // UPDATE NOTE
        public async Task UpdateAsync(Note note)
        {
            const string sql = @"
                UPDATE lending.note
                SET title = @title,
                    body = @body
                WHERE note_id = @note_id;";

            await using var command = _dataSource.CreateCommand(sql);

            command.Parameters.AddWithValue("title", note.title);
            command.Parameters.AddWithValue(
                "body", (object?)note.body ?? DBNull.Value);
            command.Parameters.AddWithValue("note_id", note.note_id);

            await command.ExecuteNonQueryAsync();
        }

        // DELETE NOTE
        public async Task DeleteAsync(long id)
        {
            const string sql = @"
                DELETE FROM lending.note
                WHERE note_id = @id;";

            await using var command = _dataSource.CreateCommand(sql);
            command.Parameters.AddWithValue("id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}
