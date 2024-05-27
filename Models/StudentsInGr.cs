using SQLite;

namespace TIMP.Models;

public class StudentsInGr
{
    [PrimaryKey, AutoIncrement]
    public int StudentsIngr_ID {get;set;}
    [Indexed]
    public int studentgr_ID {get;set;}
    [Indexed]
    public int Student_ID {get;set;}
}