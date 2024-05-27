using SQLite;

namespace TIMP.Models;

public class Faculty
{
    [PrimaryKey, AutoIncrement]
    public int Faculty_ID { get; set; }
    public string facultyShortName { get; set; }
    public string facultyFullName { get; set; }
    
}