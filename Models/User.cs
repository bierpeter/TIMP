using SQLite;

namespace TIMP.Models;

public class User
{
    [PrimaryKey] public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public int UserID { get; set; }
}