namespace library.Models;

// One Book object holds one row of lending.book.
public class Member
{
    public long Member_id { get; set; }          // member_id   BIGINT, the primary key
    public string Full_Name { get; set; } = "";   // name     VARCHAR(100) NOT NULL
    public string? Email { get; set; }     // email  VARCHAR(100), may be NULL
    public string? Member_type { get; set; }       // phone     VARCHAR(20), may be NULL

}
