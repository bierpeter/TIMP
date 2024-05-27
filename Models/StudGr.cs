using SQLite;

namespace TIMP.Models;

public class StudGr
{
    [PrimaryKey, AutoIncrement]
    public int studentgr_ID {get;set;}
    public int grNumber {get;set;}
    public int grSubnumber {get;set;}
}