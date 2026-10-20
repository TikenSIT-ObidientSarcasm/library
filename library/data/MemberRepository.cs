using library.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace library.data;

// Every SQL statement about members lives in this one class.
public class MemberRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public MemberRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    // Copies the current row into a Members object.
    private static Member ReadMember(NpgsqlDataReader reader)
    {
        return new Member
        {
            Member_id = reader.GetInt64(0),
            Full_Name = reader.GetString(1),
            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
            Member_type = reader.IsDBNull(3) ? null : reader.GetString(3)
        };
    }

    // READ: every member, sorted by name.
    public async Task<List<Member>> GetAllAsync()
    {
        const string sql =
            "SELECT member_id, full_name, email, member_type " +
            "FROM lending.member ORDER BY full_name;";

        var member = new List<Member>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            member.Add(ReadMember(reader));
        }

        return member;
    }

    private readonly MemberRepository _member;

}