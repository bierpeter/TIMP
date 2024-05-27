using SQLite;

namespace TIMP.Models;

public class Subject
{
    [PrimaryKey, AutoIncrement]
    public int Subject_ID {get; set;}
    public string subjectFullName {get; set;}
    public string subjectShortName {get; set;}
    [Indexed]
    public int   Employee_ID {get; set;}
    [Indexed]
    public int Faculty_ID {get; set;}
}
