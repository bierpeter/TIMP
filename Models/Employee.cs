using SQLite;

namespace TIMP.Models;

public class Employee
{
    [PrimaryKey, AutoIncrement]
    public int Employee_ID { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string fatherName { get; set; }
    public string mail { get; set; }
    public string phoneNumber { get; set; }
    public string DOB { get; set; }
    public string position { get; set; }
}