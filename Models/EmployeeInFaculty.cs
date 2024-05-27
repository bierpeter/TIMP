using SQLite;

namespace TIMP.Models;

public class EmployeeInFaculty
{
    [PrimaryKey, AutoIncrement]
    public int EmployeeInFaculty_ID { get; set; }
    [Indexed]
    public int Faculty_ID {get; set;}
    [Indexed]
    public int Employee_ID {get; set;}
}
