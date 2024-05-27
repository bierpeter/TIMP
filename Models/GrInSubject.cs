using SQLite;

namespace TIMP.Models;

public class GrInSubject
{
    [PrimaryKey, AutoIncrement]
    public int grInSubject_ID  {get; set;}
    [Indexed]
    public int studentgr_ID {get; set;}
    [Indexed]
    public int Subject_ID {get; set;}
}